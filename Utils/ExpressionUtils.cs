using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThreeDISevenZeroR.SpeechSequencer.Core;

// TODO: Переписать всё нахер

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class ExpressionReader
    {
        private string m_text;
        private int m_position;

        public ExpressionReader(string text)
        {
            m_text = text;
        }

        public ExpressionReader Clone()
        {
            ExpressionReader reader = new ExpressionReader(m_text);
            reader.m_position = m_position;
            return reader;
        }

        public int Peek()
        {
            if (m_position < m_text.Length)
            {
                return m_text[m_position];
            }
            else
            {
                return -1;
            }
        }
        public int Read()
        {
            if (m_position < m_text.Length)
            {
                return m_text[m_position++];
            }
            else
            {
                return -1;
            }
        }
    }

    public static class ExpressionUtils
    {
        public const char c_aliasExpression = '@';
        public const char c_variableExpression = '$';
        public const char c_argumentSeparator = ';';

        public static IAudioNode EvaluateFreeformExpressionAsAudio(string expression, IPlaybackContext context)
        {
            ISequenceNode node = EvaluateFreeformExpression(new ExpressionReader(expression), context);

            if (node is IAudioNode)
            {
                return (IAudioNode)node;
            }
            else if (node is IValueNode)
            {
                return WrapValueAsSpeech((IValueNode)node);
            }

            return null;
        }
        public static ISequenceNode EvaluateExpression(ExpressionReader reader, IPlaybackContext context)
        {
            string name;
            int expressionType = reader.Read();

            name = TryReadExpressionName(reader);

            if (!string.IsNullOrWhiteSpace(name))
            {
                if (expressionType == c_aliasExpression)
                {
                    int argIndex = 0;
                    bool hasArgs = TryFindArgs(reader);

                    IAlias alias = ResourceManager.Instance.GetAlias(name);
                    IAliasEntryNode aliasNode = alias.CreateNode();

                    while (hasArgs)
                    {
                        ISequenceNode node = EvaluateFreeformExpression(reader, context, true, ref hasArgs);

                        if (alias != null && argIndex < alias.ArgumentCount)
                        {
                            aliasNode.OverrideVariableCreator(alias.GetAliasArgumentName(argIndex), () => node);
                        }

                        argIndex++;
                    }

                    for (int i = argIndex; i < alias.ArgumentCount; i++)
                    {
                        aliasNode.OverrideVariableCreator(alias.GetAliasArgumentName(i), alias.GetDefaultArgumentValue(i));
                    }

                    return aliasNode;
                }
                else
                {
                    return context.GetVariableNode(name);
                }
            }
            else
            {
                return new TextValueNode(((char)expressionType).ToString());
            }
        }
        public static ISequenceNode EvaluateFreeformExpression(ExpressionReader reader, IPlaybackContext context)
        {
            bool dummy = false;
            return EvaluateFreeformExpression(reader, context, false, ref dummy);
        }
        public static ISequenceNode EvaluateFreeformExpression(ExpressionReader reader, IPlaybackContext context, bool asArgument, ref bool hasArgs)
        {
            List<ISequenceNode> nodes = new List<ISequenceNode>();
            StringBuilder valueBuilder = new StringBuilder();
            char prevChar = '\0';

            while (true)
            {
                int charIndex = reader.Peek();
                char c = (char)charIndex;

                if (charIndex != -1)
                {
                    if (prevChar == '\\')
                    {
                        valueBuilder.Append(c);
                        reader.Read();
                    }
                    else if (c != '\\')
                    {
                        if (asArgument && (c == c_argumentSeparator || c == ')'))
                        {
                            if (c == ')')
                            {
                                hasArgs = false;
                            }

                            reader.Read();
                            break;
                        }

                        switch (c)
                        {
                            case c_aliasExpression:
                            case c_variableExpression:
                                {
                                    if (valueBuilder.Length != 0)
                                    {
                                        nodes.Add(new TextValueNode(valueBuilder.ToString()));
                                        valueBuilder.Clear();
                                    }

                                    nodes.Add(EvaluateExpression(reader, context));

                                    break;
                                }
                            default:
                                {
                                    valueBuilder.Append(c);
                                    reader.Read();
                                    break;
                                }
                        }
                    }

                    prevChar = c;
                }
                else
                {
                    if (asArgument)
                    {
                        hasArgs = false;
                    }

                    break;
                }
            }

            if (valueBuilder.Length != 0)
            {
                nodes.Add(new TextValueNode(valueBuilder.ToString()));
            }

            return AssembleResultNode(nodes);
        }

        private static string TryReadExpressionName(ExpressionReader reader)
        {
            StringBuilder nameBuilder = new StringBuilder();
            bool nameEnd = false;

            while (true)
            {
                int charIndex = reader.Peek();

                if (!char.IsLetterOrDigit((char)charIndex))
                {
                    nameEnd = true;

                    if (charIndex != '(')
                    {
                        reader.Read();
                    }
                }
                else
                {
                    reader.Read();
                }

                if (!nameEnd)
                {
                    nameBuilder.Append((char)charIndex);
                }
                else
                {
                    return nameBuilder.ToString();
                }
            }
        }
        private static bool TryFindArgs(ExpressionReader reader)
        {
            while (true)
            {
                int charIndex = reader.Peek();

                if (charIndex != -1)
                {
                    if (!char.IsWhiteSpace((char)charIndex))
                    {
                        if (charIndex == '(')
                        {
                            reader.Read();
                            return true;
                        }

                        break;
                    }
                    else
                    {
                        reader.Read();
                    }
                }
                else
                {
                    break;
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
                        if (node is IValueNode)
                        {
                            sequential.AddNode(WrapValueAsSpeech((IValueNode)node));
                        }
                        else
                        {
                            sequential.AddNode((IAudioNode)node);
                        }
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
        public static IAudioNode WrapValueAsSpeech(string value)
        {
            TTSNode node = new TTSNode();
            node.ValueHolder = new TextValueNode(value);
            return node;
        }
        public static IAudioNode WrapValueAsSpeech(IValueNode value)
        {
            TTSNode node = new TTSNode();
            node.ValueHolder = value;
            return node;
        }
    }
}