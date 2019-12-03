using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace e_sidoc
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            const string rootElemTag = "FICHES_XML";
            const string nodePath = "/" + rootElemTag + "/EMPRUNTEURS";
            const string idTag = "IDENTITE_ENT_M";
            Dictionary<string, XmlNode> users = new Dictionary<string, XmlNode>();

            void lit(XmlDocument xml)
            {
                foreach (XmlNode n in xml.SelectNodes(nodePath))
                {
                    var nn = n.SelectSingleNode(idTag);
                    string id = nn.InnerText.Trim();
                    if (!users.ContainsKey(id))
                    {
                        users.Add(id, n.Clone());
                    }
                }
            }

            var basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            var xmlFile = new XmlDocument();
            xmlFile.Load(basePath + @"\temp\Lavoisier\exportEsidoc eleves lycee.xml");
            lit(xmlFile);
            xmlFile = new XmlDocument();
            xmlFile.Load(basePath + @"\temp\Lavoisier\exportEsidoc eleves clg.xml");
            lit(xmlFile);

            var xmlFileO = new XmlDocument();
            var root = xmlFileO.CreateElement(rootElemTag);
            foreach(XmlNode n in users.Values)
            {
                root.AppendChild(xmlFileO.ImportNode(n, true));
            }

            
            xmlFileO.AppendChild(root);

            var settings = new XmlWriterSettings();
            settings.Encoding = Encoding.GetEncoding("windows-1252");
            settings.Indent = true;
            settings.IndentChars = "	";
            using (var writer = XmlWriter.Create(basePath + @"\temp\Lavoisier\exportEsidoc eleves Lyc clg.xml", settings))
            {
                xmlFileO.Save(writer);
            }

        }
    }
}
