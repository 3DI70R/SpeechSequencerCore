using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core.Model.SequenceTree.Implementation.Audio
{
    [XmlElementBinding("Expression")]
    [Description("Воспроизводит выражение")]
    public class ExpressionNode : ValuePlaybackNode
    {
        protected override ISampleProvider CreateProvider(string value, IPlaybackContext context)
        {
            IAudioNode node;
            
            node = ExpressionParser.ParseExpression(value, context)().ToAudio();
            node.InitNewState(context);

            return node;
        }
    }
}
