using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class ResourceManager
    {
        public const string VariableNamespace = "Variable";
        private const string c_resourcesFileName = "Resources.xml";

        private static ResourceManager s_instance = new ResourceManager();
        public static ResourceManager Instance { get { return s_instance; } }

        private Dictionary<string, string> m_constants = new Dictionary<string, string>();
        private Dictionary<string, Alias> m_aliases = new Dictionary<string, Alias>();

        private ResourceManager() { }
        public void LoadDefaultResources()
        {
            string filePath = Path.Combine(Directory.GetCurrentDirectory(), c_resourcesFileName);

            if(File.Exists(filePath))
            {
                AppendResources(filePath);
            }
            else
            {
                CreateEmptyResourceFile(filePath);
            }
        }

        public string GetConstant(string name)
        {
            return m_constants[name];
        }
        public Alias GetAlias(string aliasName)
        {
            return m_aliases[aliasName.ToLowerInvariant()];
        }

        public void AppendResources(string filePath)
        {
            XmlDocument document = new XmlDocument();
            document.Load(filePath);
            ParseResources(document.DocumentElement);
        }
        public void ParseResources(XmlElement resourcesElement)
        {
            foreach(XmlNode node in resourcesElement)
            {
                if(node.NodeType == XmlNodeType.Element)
                {
                    XmlElement element = (XmlElement)node;

                    switch (element.Name)
                    {
                        case "Constant":
                            string name = element.GetAttribute("Name");
                            string value = element.GetAttribute("Value");
                            m_constants[name] = value;
                            break;
                        case "Alias":
                            Alias alias = new Alias();
                            alias.InitFromXml(element);
                            m_aliases[alias.Name] = alias;
                            break;
                    }
                }
            }
        }

        public static void CreateEmptyResourceFile(string path)
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("Resources");
            XmlComment comment = document.CreateComment("Here you can define resources used by Speech Sequencer");

            root.AppendChild(comment);
            document.AppendChild(root);
            document.DocumentElement.SetAttribute("xmlns:var", VariableNamespace);

            document.Save(path);
        }
    }
}
