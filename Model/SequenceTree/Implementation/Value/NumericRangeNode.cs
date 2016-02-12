using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("NumericRange")]
    public class NumericRangeNode : ValueNode
    {
        [XmlAttributeBinding]
        public bool Float { get; set; } = false;

        [XmlAttributeBinding]
        public float From { get; set; } = 0;

        [XmlAttributeBinding]
        public float To { get; set; } = 1;

        public override string LoadValue(IPlaybackContext context)
        {
            if(Float)
            {
                return (From + context.SharedRandom.NextDouble() * (To - From)).ToString(CultureInfo.InvariantCulture.NumberFormat);
            }
            else
            {
                return context.SharedRandom.Next((int) From, (int) To).ToString();
            }
        }
    }
}
