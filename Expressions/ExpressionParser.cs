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

        public static ParseTree ParseExpressionToTree(string text)
        {
            return parser.Parse(text);
        }
        public static Func<ISequenceNode> ParseExpression(string expression)
        {
            return ParseExpression(expression, new Context());
        }
        public static Func<ISequenceNode> ParseExpression(string expression, Context context)
        {
            ParseTree tree = ParseExpressionToTree(expression);

            if(tree.Status == ParseTreeStatus.Error)
            {
                throw new FormatException("Invalid Expression " + tree.ParserMessages[0].Message);
            }

            return BuildExpressionCreator(tree.Root, context);
        }

        private static Func<ISequenceNode> BuildExpressionCreator(ParseTreeNode tree, Context context)
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

                return SequenceFactory.AssembleResultNode(nodes);
            };
        }
        private static Func<ISequenceNode> BuildTextCreator(ParseTreeNode node, Context context)
        {
            string text = node.Token.Text;

            return () =>
            {
                return new TextValueNode(text);
            };
        }
        private static Func<ISequenceNode> BuildAliasCreator(ParseTreeNode node, Context context)
        {
            ParseTreeNode nameNode = FindNode(node, SpeechExpression.NameLiteralName);
            ParseTreeNode argsNode = FindNode(node, SpeechExpression.ArgumentListLiteralName);

            string aliasName = nameNode.Token.Text;
            Func<ISequenceNode>[] args;

            if (argsNode != null)
            {
                ParseTreeNodeList argsChildNodes = argsNode.ChildNodes;
                args = new Func<ISequenceNode>[argsChildNodes.Count];

                for (int i = 0; i < argsChildNodes.Count; i++)
                {
                    args[i] = BuildExpressionCreator(argsChildNodes[i], context);
                }
            }
            else
            {
                args = new Func<ISequenceNode>[0];
            }

            Alias alias = ResourceManager.Instance.GetAlias(aliasName);

            return () =>
            {
                return alias.CreateNode(context, args);
            };
        }
        private static Func<ISequenceNode> BuildVariableCreator(ParseTreeNode node, Context context)
        {
            ParseTreeNode nameNode = FindNode(node, SpeechExpression.NameLiteralName);

            string varName = nameNode.Token.Text;

            return () =>
            {
                return context.GetVariable(varName);
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
