using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    [XmlElementBinding("RandomFile")]
    [Description("Возвращает путь к случайному файлу лежащему в папке")]
    public class RandomFilePathNode : ValueNode
    {
        [XmlAttributeBinding]
        [Description("Папка в которой необходимо выбирать файлы")]
        public string Path { get; set; }

        [XmlAttributeBinding]
        [Description("Шаблон названия файла")]
        public string Pattern { get; set; } = "*";

        protected override string InitValue(Context context)
        {
            string[] files = Directory.EnumerateFiles(Path, Pattern).ToArray();

            if(files.Length == 0)
            {
                return "";
            }
            else
            {
                return files[context.SharedRandom.Next(0, files.Length)];
            }
        }
    }
}
