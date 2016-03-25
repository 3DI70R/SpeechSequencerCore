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

        private const string c_configOptionsFile = "Config.xml";
        private const string c_resourcesFolder = "Resources";

        private const string c_configAliasCollectionNode = "AliasCollection";
        private const string c_configOptionsCollectionNode = "Options";
        private const string c_configOptionsConstantsNode = "Constants";
        private const string c_configVoicesCollectionNode = "Voices";

        private static ResourceManager s_instance = new ResourceManager();
        public static ResourceManager Instance { get { return s_instance; } }

        private Dictionary<string, string> m_constants = new Dictionary<string, string>();
        private Dictionary<string, Alias> m_aliases = new Dictionary<string, Alias>();

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
            root.AppendChild(document.CreateElement(c_configOptionsConstantsNode));
            root.AppendChild(document.CreateElement(c_configOptionsCollectionNode));
            root.AppendChild(document.CreateElement(c_configAliasCollectionNode));
            root.AppendChild(document.CreateElement(c_configVoicesCollectionNode));
            document.DocumentElement.SetAttribute("xmlns:var", VariableNamespace);

            document.Save(path);
        }

        public void AppendConstants(XmlElement element)
        {
            foreach(XmlNode node in element.ChildNodes)
            {
                if(node is XmlElement)
                {
                    try
                    {
                        XmlElement child = (XmlElement)node;
                        string key = child.GetAttributeNode("Name").Value;
                        string value = child.InnerText;

                        m_constants[key] = value;
                    }
                    catch(Exception e)
                    {
                        Debug.Print(e.ToString());
                    }
                }
            }
        }
        public string GetConstant(string name)
        {
            return m_constants[name];
        }

        public void AppendAliases(XmlElement element)
        {
            /*if (element != null)
            {
                foreach (IAlias alias in ObjectFactory.Instance.CreateChildrenAlias(element))
                {
                    m_aliases[alias.Name.ToLowerInvariant()] = alias;
                }
            }*/
        }
        public Alias GetAlias(string aliasName)
        {
            return m_aliases[aliasName.ToLowerInvariant()];
        }

        /*private XmlDocument LoadDocument(string path)
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

        */
        }
    }
