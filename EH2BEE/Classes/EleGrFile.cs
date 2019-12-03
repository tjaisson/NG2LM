using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Classes
{
    class TEleGrFile : XmlDocument
    {
        private static class PN
        {
            public const string ELEGROUP_tag = "IMPORT_ELEVES";
            public const string version_tag = "VERSION";

            public const string PARAMETRES_tag = "PARAMETRES";
            public const string UAJ_tag = "UAJ";
            public const string ANNEE_SCOLAIRE_tag = "ANNEE_SCOLAIRE";
            public const string DATE_EXPORT_tag = "DATE_EXPORT";
            public const string DATE_IMPORT_tag = "DATE_IMPORT";
            public const string HORODATAGE_tag = "HORODATAGE";
            public const string NUM_ENVOI_tag = "NUM_ENVOI";
            public const string LOGICIEL_tag = "LOGICIEL";

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

            public const string GROUPES_tag = "GROUPES";
            public const string GROUPE_tag = "GROUPE";
            public const string CODE_GROUPE_tag = "CODE_GROUPE";
            public const string DATE_DEBUT_GROUPE_tag = "DATE_DEBUT_GROUPE";
            public const string DATE_FIN_GROUPE_tag = "DATE_FIN_GROUPE";
        }


        private XmlNode eleves;

        public void Init()
        {
            /*
             * <UAJ>0750702F</UAJ>
                <ANNEE_SCOLAIRE>2019</ANNEE_SCOLAIRE>
                <DATE_IMPORT>04/09/2019</DATE_IMPORT>
                <NUM_ENVOI>0</NUM_ENVOI>
                <LOGICIEL>EDT</LOGICIEL>
             */


            var de = CreateElement(PN.ELEGROUP_tag);
            AppendChild(de);
            var att = CreateAttribute(PN.version_tag);
            att.Value = "1.4";
            de.Attributes.Append(att);

            var param = CreateElement(PN.PARAMETRES_tag);
            var n = CreateElement(PN.UAJ_tag);
            n.AppendChild(CreateTextNode("0750677D"));
            param.AppendChild(n);
            n = CreateElement(PN.ANNEE_SCOLAIRE_tag);
            n.AppendChild(CreateTextNode("2019"));
            param.AppendChild(n);
            n = CreateElement(PN.DATE_IMPORT_tag);
            n.AppendChild(CreateTextNode("10/10/2019"));
            param.AppendChild(n);
            n = CreateElement(PN.NUM_ENVOI_tag);
            n.AppendChild(CreateTextNode("0"));
            param.AppendChild(n);
            n = CreateElement(PN.LOGICIEL_tag);
            n.AppendChild(CreateTextNode("EDT"));
            param.AppendChild(n);
            de.AppendChild(param);
            /*
             * <DONNEES>
                <ELEVES>
                <ELEVE>
                <ELEVE_ID>1633147</ELEVE_ID>
                <NOM_DE_FAMILLE>ABDELALI</NOM_DE_FAMILLE>
                <PRENOM>Aida - Amel</PRENOM>
                <DATE_NAISS>2002-11-12</DATE_NAISS>
                <GROUPES>
                <GROUPE>
                <CODE_GROUPE>TEPSLUMA</CODE_GROUPE>
                <DATE_DEBUT_GROUPE>2019-09-02</DATE_DEBUT_GROUPE>
                <DATE_FIN_GROUPE>2020-07-04</DATE_FIN_GROUPE>
                </GROUPE>
             */
            var dt = CreateElement(PN.DONNEES_tag);
            eleves = CreateElement(PN.ELEVES_tag);
            dt.AppendChild(eleves);
            de.AppendChild(dt);
        }

        public void AddEleve(EducCsvReader.TEducUser eh, TBEEFile.TBEEUser bee)
        {
            Debug.Assert(eleves != null);
            var el = CreateElement(PN.ELEVE_tag);
            var n = CreateElement(PN.ELEVE_ID_tag);
            n.AppendChild(CreateTextNode(bee.UID2));
            el.AppendChild(n);
            n = CreateElement(PN.NOM_tag);
            n.AppendChild(CreateTextNode(bee.nom));
            el.AppendChild(n);
            n = CreateElement(PN.PRENOM_tag);
            n.AppendChild(CreateTextNode(bee.prenom));
            el.AppendChild(n);
            if (!string.IsNullOrEmpty(bee.prenom2))
            {
                n = CreateElement(PN.PRENOM2_tag);
                n.AppendChild(CreateTextNode(bee.prenom2));
                el.AppendChild(n);
            }
            if (!string.IsNullOrEmpty(bee.prenom3))
            {
                n = CreateElement(PN.PRENOM3_tag);
                n.AppendChild(CreateTextNode(bee.prenom3));
                el.AppendChild(n);
            }
            n = CreateElement(PN.DATE_NAISS_tag);
            n.AppendChild(CreateTextNode(bee.DateNaiss.ToString("yyyy-MM-dd")));
            el.AppendChild(n);

            var grps = CreateElement(PN.GROUPES_tag);
            foreach  (var gname in eh.Groups)
            {
                var g = CreateElement(PN.GROUPE_tag);
                n = CreateElement(PN.CODE_GROUPE_tag);
                n.AppendChild(CreateTextNode(gname));
                g.AppendChild(n);
                n = CreateElement(PN.DATE_DEBUT_GROUPE_tag);
                n.AppendChild(CreateTextNode("2019-09-02"));
                g.AppendChild(n);
                n = CreateElement(PN.DATE_FIN_GROUPE_tag);
                n.AppendChild(CreateTextNode("2020-07-04"));
                g.AppendChild(n);
                grps.AppendChild(g);
            }
            el.AppendChild(grps);
            eleves.AppendChild(el);
        }

        public void SaveToFile(string fn)
        {
            var settings = new XmlWriterSettings();
            settings.Encoding = Encoding.GetEncoding("ISO-8859-15");
            settings.Indent = true;
            settings.IndentChars = "";
            using (var writer = XmlWriter.Create(fn, settings))
            {
                Save(writer);
            }
        }

    }
}
