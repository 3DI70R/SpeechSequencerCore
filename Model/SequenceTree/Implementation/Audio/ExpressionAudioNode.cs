using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NAudio.Wave;
using System.ComponentModel;

namespace ThreeDISevenZeroR.SpeechSequencer.Core.Model.SequenceTree.Implementation.Audio
{
    [XmlElementBinding("ExpressionAsAudio")]
    [Description("Воспроизводит выражение")]
    public class ExpressionAudioNode : ValuePlaybackNode
    {
        protected override ISampleProvider CreateProvider(string value, Context context)
        {
            IAudioNode node;
            
            node = ExpressionParser.ParseExpression(value, context)().ToAudio();
            node.InitNewState(context);

            return node;
        }
    }
}
