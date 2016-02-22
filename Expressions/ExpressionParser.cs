using Irony.Interpreter;
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public static class ExpressionParser
    {
        private static readonly SpeechExpression expression = new SpeechExpression();
        private static readonly Parser parser = new Parser(expression);

        public static Func<ISequenceNode> ParseExpression(string expression)
        {
            return ParseExpression(expression, new PlaybackContext());
        }
        public static Func<ISequenceNode> ParseExpression(string expression, IPlaybackContext context)
        {
            ParseTree tree = parser.Parse(expression);

            if(tree.Root.IsError)
            {
                throw new FormatException("Invalid Expression " + tree.Root.Comments);
            }

            return BuildExpressionCreator(tree.Root, context);
        }

        public static bool HasAudioNode(List<ISequenceNode> nodes)
        {
            foreach (ISequenceNode node in nodes)
            {
                if (node is IAudioNode)
                {
                    return true;
                }
            }

            return false;
        }
        public static ISequenceNode AssembleResultNode(List<ISequenceNode> nodes)
        {
            if (nodes.Count == 0)
            {
                return new TextValueNode();
            }
            else if (nodes.Count == 1)
            {
                return nodes[0];
            }
            else
            {
                if (HasAudioNode(nodes))
                {
                    SequentialAudioNode sequential = new SequentialAudioNode();

                    foreach (ISequenceNode node in nodes)
                    {
                        sequential.AddNode(node.ToAudio());
                    }

                    return sequential;
                }
                else
                {
                    JoinValueNode join = new JoinValueNode();

                    foreach (ISequenceNode node in nodes)
                    {
                        join.AddNode((IValueNode)node);
                    }

                    return join;
                }
            }
        }

        private static Func<ISequenceNode> BuildExpressionCreator(ParseTreeNode tree, IPlaybackContext context)
        {
            List<Func<ISequenceNode>> funcList = new List<Func<ISequenceNode>>();

            foreach (ParseTreeNode node in tree.ChildNodes)
            {
                switch (node.Term.Name)
                {
                    case SpeechExpression.TextLiteralName:
                        funcList.Add(BuildTextCreator(node, context));
                        break;
                    case SpeechExpression.AliasLiteralName:
                        funcList.Add(BuildAliasCreator(node, context));
                        break;
                    case SpeechExpression.VariableLiteralName:
                        funcList.Add(BuildVariableCreator(node, context));
                        break;
                }
            }

            return () =>
            {
                List<ISequenceNode> nodes = new List<ISequenceNode>();

                foreach (Func<ISequenceNode> creator in funcList)
                {
                    nodes.Add(creator());
                }

                return AssembleResultNode(nodes);
            };
        }
        private static Func<ISequenceNode> BuildTextCreator(ParseTreeNode node, IPlaybackContext context)
        {
            string text = node.Token.Text;

            return () =>
            {
                return new TextValueNode(text);
            };
        }
        private static Func<ISequenceNode> BuildAliasCreator(ParseTreeNode node, IPlaybackContext context)
        {
            ParseTreeNode nameNode = FindNode(node, SpeechExpression.NameLiteralName);
            ParseTreeNode argsNode = FindNode(node, SpeechExpression.ArgumentListLiteralName);

            string aliasName = nameNode.Token.Text;
            List<Func<ISequenceNode>> funcCreators = new List<Func<ISequenceNode>>();

            if(argsNode != null)
            {
                foreach (ParseTreeNode argument in argsNode.ChildNodes)
                {
                    funcCreators.Add(BuildExpressionCreator(argument, context));
                }
            }

            IAlias alias = ResourceManager.Instance.GetAlias(aliasName);

            return () =>
            {
                IAliasEntryNode entry = alias.CreateNode(context);

                for (int i = 0; i < alias.ArgumentCount; i++)
                {
                    string argName = alias.GetAliasArgumentName(i);
                    Func<ISequenceNode> creator = i < funcCreators.Count ? funcCreators[i] : alias.GetDefaultArgumentValue(i);
                    entry.OverrideVariableCreator(argName, creator);
                }

                return entry;
            };
        }
        private static Func<ISequenceNode> BuildVariableCreator(ParseTreeNode node, IPlaybackContext context)
        {
            ParseTreeNode nameNode = FindNode(node, SpeechExpression.NameLiteralName);

            string varName = nameNode.Token.Text;

            return () =>
            {
                return context.GetVariableNode(varName);
            };
        }
        private static ParseTreeNode FindNode(ParseTreeNode node, string termName)
        {
            foreach (ParseTreeNode child in node.ChildNodes)
            {
                if (child.Term.Name == termName)
                {
                    return child;
                }
            }

            return null;
        }
    }
}
