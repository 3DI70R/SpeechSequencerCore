using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core.Model.SequenceTree.Implementation.Audio
{
    [XmlElementBinding("ExpressionAsValue")]
    [Description("Загружает значение выражения")]
    public class ExpressionValueNode : ValueModifierNode
    {
        public override string ProcessValue(string value)
        {
            IValueNode node;

            node = (IValueNode) ExpressionParser.ParseExpression(value, LocalContext)();
            node.InitNewState(LocalContext);

            return node.Value;
        }
    }
}
