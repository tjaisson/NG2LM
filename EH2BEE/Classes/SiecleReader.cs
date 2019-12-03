using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Classes
{
    /// <summary>
    /// Classe qui permet la lecture du fichier xml elevesAvecAdresses issu de siècle BEE
    /// (base élèves de l'établissement)
    /// </summary>
    public class TBEEFile : IEnumerable<TBEEFile.TBEEUser>
    {
        public abstract class TBEEUser : TUser
        {
            public string prenom2;
            public string prenom3;
            public string UID;
            public string UID2;
            public DateTime DateSortie;

        }

        private class TInnerBEEUser : TBEEUser
        {

        }

        private static class PN
        {
            public const string SCONET_tag = "BEE_ELEVES";

            public const string PARAMETRES_tag = "PARAMETRES";
            public const string UAJ_tag = "UAJ";
            public const string ANNEE_SCOLAIRE_tag = "ANNEE_SCOLAIRE";
            public const string DATE_EXPORT_tag = "DATE_EXPORT";
            public const string HORODATAGE_tag = "HORODATAGE";

            public const string DONNEES_tag = "DONNEES";
            public const string ELEVES_tag = "ELEVES";
            public const string ELEVE_tag = "ELEVE";
            public const string NOM_tag = "NOM_DE_FAMILLE";
            public const string PRENOM_tag = "PRENOM";
            public const string PRENOM2_tag = "PRENOM2";
            public const string PRENOM3_tag = "PRENOM3";
            public const string DATE_NAISS_tag = "DATE_NAISS";
            public const string DATE_SORTIE_tag = "DATE_SORTIE";
            public const string ELEVE_ID_tag = "ELEVE_ID";
            public const string ELENOET_tag = "ELENOET";
            public const string ID_NATIONAL_tag = "ID_NATIONAL";
            public const string STRUCTURES_tag = "STRUCTURES";
            public const string STRUCTURE_tag = "STRUCTURE";
            public const string STRUCTURES_ELEVE_tag = "STRUCTURES_ELEVE";
            public const string CODE_STRUCTURE_tag = "CODE_STRUCTURE";
        }

        string fileName;



        public TBEEFile(string filename)
        {
            fileName = filename;
        }

        /*private string getParam(string p)
        {
            XmlNode n;
            return (n = xmlFile.SelectSingleNode("/" + PN.SCONET_tag + "/" + PN.PARAMETRES_tag + "/" + p)) == null ? "" : n.InnerXml.Trim();
        }

        public string Annee
        { get { return getParam(PN.ANNEE_SCOLAIRE_tag); } }

        public string Etab
        { get { return getParam(PN.UAJ_tag); } }

        public string Date
        { get { return getParam(PN.DATE_EXPORT_tag); } }

        public string Horodatage
        { get { return getParam(PN.HORODATAGE_tag); } }
        */


        public IEnumerator<TBEEUser> GetEnumerator()
        {
            var xmlFile = new XmlDocument();
            var ext = Path.GetExtension(fileName);
            if (ext.Equals(@".xml", StringComparison.OrdinalIgnoreCase))
            {
                xmlFile.Load(fileName);
                return Enum(xmlFile);
            }
            else
            {
                if (ext.Equals(@".zip", StringComparison.OrdinalIgnoreCase))
                {
                    using (var fs = new FileStream(fileName, FileMode.Open))
                    {
                        using (var zf = new ZipArchive(fs, ZipArchiveMode.Read))
                        {
                            if (zf.Entries.Count != 1)
                            {
                                throw new Exception();
                            }
                            var ze = zf.Entries[0];
                            xmlFile.Load(ze.Open());
                            return Enum(xmlFile);
                        }
                    }
                }
                else
                {
                    throw new Exception();
                }
            }
        }

        private MyHashSet DivisionNames;

        private IEnumerator<TBEEUser> Enum(XmlDocument xmlFile)
        {
            if (xmlFile.DocumentElement.Name != PN.SCONET_tag) throw new Exception();
            DivisionNames = new MyHashSet();
            XmlNode structsNode = xmlFile.SelectSingleNode("/" + PN.SCONET_tag + "/" + PN.DONNEES_tag + "/" + PN.STRUCTURES_tag);
            XmlNodeList elevesNode = xmlFile.SelectNodes("/" + PN.SCONET_tag + "/" + PN.DONNEES_tag + "/" + PN.ELEVES_tag + "/" + PN.ELEVE_tag);

            string p, s;
            XmlNode t;
            foreach (XmlNode i in elevesNode)
            {
                s = i.Attributes[PN.ELEVE_ID_tag].Value;
                p = PN.STRUCTURES_ELEVE_tag + "[@" + PN.ELEVE_ID_tag + "=\"" + s + "\"]/" + PN.STRUCTURE_tag + "/" + PN.CODE_STRUCTURE_tag;
                if ((t = structsNode.SelectSingleNode(p)) != null)
                {
                    string g = t.InnerText.Trim();
                    if (! string.IsNullOrEmpty(g))
                    {
                        g = DivisionNames.Getvalue(g);
                        string DateNaissString = (t = i.SelectSingleNode(PN.DATE_NAISS_tag)) == null ? "" : t.InnerText.Trim();
                        DateTime DateNaiss;
                        if (!DateTime.TryParse(DateNaissString, out DateNaiss)) DateNaiss = DateTime.MinValue;
                        string DateSortieString = (t = i.SelectSingleNode(PN.DATE_SORTIE_tag)) == null ? "" : t.InnerText.Trim();
                        DateTime DateSortie;
                        if (!DateTime.TryParse(DateSortieString, out DateSortie)) DateSortie = DateTime.MinValue;


                        yield return new TInnerBEEUser
                        {
                            division = g,
                            nom = (t = i.SelectSingleNode(PN.NOM_tag)) == null ? "" : t.InnerText.Trim(),
                            prenom = (t = i.SelectSingleNode(PN.PRENOM_tag)) == null ? "" : t.InnerText.Trim(),
                            prenom2 = (t = i.SelectSingleNode(PN.PRENOM2_tag)) == null ? "" : t.InnerText.Trim(),
                            prenom3 = (t = i.SelectSingleNode(PN.PRENOM3_tag)) == null ? "" : t.InnerText.Trim(),
                            DateNaiss = DateNaiss,
                            UID = i.Attributes[PN.ELENOET_tag].Value.Trim(),
                            UID2 = s.Trim(),
                            DateSortie = DateSortie
                        };
                    }
                }
            }
            DivisionNames = null;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

}
