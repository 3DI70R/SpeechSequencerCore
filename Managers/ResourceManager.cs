using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ThreeDISevenZeroR.SpeechSequencer.Core
{
    public class ResourceManager
    {
        private const string c_configOptionsFile = "Config.xml";
        private const string c_resourcesFolder = "Resources";
        private const string c_configAliasCollectionNode = "AliasCollection";
        private const string c_configOptionsCollectionNode = "Options";
        private const string c_configOptionsExtraNode = "ExtraData";

        private static ResourceManager s_instance = new ResourceManager();

        private Dictionary<string, IAlias> m_aliases = new Dictionary<string, IAlias>();

        public static ResourceManager Instance { get { return s_instance; } }

        private ResourceManager()
        {
            /*LoadConfig(Path.Combine(Directory.GetCurrentDirectory(), c_configOptionsFile));
            LoadAdditionalResources();*/
        }

        private void CreateEmptyConfig(string path)
        {
            XmlDocument document = new XmlDocument();
            XmlElement root = document.CreateElement("Config");

            document.AppendChild(root);
            root.AppendChild(document.CreateElement(c_configAliasCollectionNode));
            root.AppendChild(document.CreateElement(c_configOptionsCollectionNode));
            root.AppendChild(document.CreateElement(c_configOptionsExtraNode));
            document.DocumentElement.SetAttribute("xmlns:var", XmlBinder.c_varNamespace);

            document.Save(path);
        }
        private XmlDocument LoadDocument(string path)
        {
            XmlDocument document = new XmlDocument();
            document.Load(path);
            return document;
        }

        public void LoadConfig(string path)
        {
            if(!File.Exists(path))
            {
                CreateEmptyConfig(path);
            }

            XmlDocument doc = LoadDocument(path);
            AppendResources(doc.DocumentElement);
        }
        public void LoadAdditionalResources()
        {
            foreach(string file in Directory.GetFiles(c_resourcesFolder))
            {
                if(file.EndsWith(".xml"))
                {
                    try
                    {
                        XmlDocument document = LoadDocument(file);
                        AppendResources(document.DocumentElement);
                    }
                    catch (Exception e)
                    {
                        Console.Write(e.Message);
                    }
                }
            }
        }

        public void AppendResources(XmlElement element)
        {
            AddAliases(element[c_configAliasCollectionNode]);
        }

        public void AddAliases(XmlElement element)
        {
            if(element != null)
            {
                foreach(IAlias alias in NodeFactory.Instance.CreateChildrenAlias(element))
                {
                    m_aliases[alias.Name] = alias;
                }
            }
        }

        public IAlias GetAlias(string aliasName)
        {
            return m_aliases[aliasName];
        }
    }
}
