using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Xml;

namespace Publi4Par
{
    /// <summary>
    /// Classe qui permet la lecture du fichier xml elevesAvecAdresses issu de siècle BEE
    /// (base élèves de l'établissement)
    /// </summary>
    public class TBEEFile : IEnumerable<TImportedUser>
    {
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


        public IEnumerator<TImportedUser> GetEnumerator()
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

        private IEnumerator<TImportedUser> Enum(XmlDocument xmlFile)
        {
            if (xmlFile.DocumentElement.Name != PN.SCONET_tag) throw new Exception();
            Dictionary<string, Tuple<string, string>> groupes = new Dictionary<string, Tuple<string, string>>();
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
                    string g = stringManip.simplifyGroupName(t.InnerText.Trim());
                    if (g != "")
                    {
                        string sg = g.ToUpperInvariant();
                        Tuple<string, string> KP;
                        if (!groupes.TryGetValue(sg, out KP))
                        {
                            KP = new Tuple<string, string>(g, sg);
                            groupes.Add(sg, KP);
                        }
                        yield return new TImportedUser
                        {
                            Groupe = KP.Item1,
                            SimpGroupe = KP.Item2,
                            Nom = (t = i.SelectSingleNode(PN.NOM_tag)) == null ? "" : t.InnerText.Trim(),
                            Prenom = (t = i.SelectSingleNode(PN.PRENOM_tag)) == null ? "" : t.InnerText.Trim(),
                            UID = i.Attributes[PN.ELENOET_tag].Value.Trim(),
                            UID2 = s.Trim(),
                        };
                    }
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }

    /// <summary>
    /// Classe qui permet la lecture du fichier xml responsablesAvecAdresses issu de siècle BEE
    /// (base élèves de l'établissement)
    /// </summary>
    public class TRAAFile : IEnumerable<TRAAUser>
    {
        private static class PN
        {
            public const string SCONET_tag = "BEE_REPONSABLES";

            public const string PARAMETRES_tag = "PARAMETRES";
            public const string UAJ_tag = "UAJ";
            public const string ANNEE_SCOLAIRE_tag = "ANNEE_SCOLAIRE";
            public const string DATE_EXPORT_tag = "DATE_EXPORT";
            public const string HORODATAGE_tag = "HORODATAGE";

            public const string DONNEES_tag = "DONNEES";
            public const string PERSONNES_tag = "PERSONNES";
            public const string PERSONNE_tag = "PERSONNE";
            public const string NOM_tag = "NOM_DE_FAMILLE";
            public const string PRENOM_tag = "PRENOM";
            public const string PERSONNE_ID_tag = "PERSONNE_ID";
            public const string ADRESSE_ID_tag = "ADRESSE_ID";
            public const string LC_CIVILITE_tag = "LC_CIVILITE";
            public const string LL_CIVILITE_tag = "LL_CIVILITE";

            public const string RESPONSABLES_tag = "RESPONSABLES";
            public const string RESPONSABLE_tag = "RESPONSABLE_ELEVE";
            public const string ELEVE_ID_tag = "ELEVE_ID";

            public const string ADRESSES_tag = "ADRESSES";
            public const string ADRESSE_tag = "ADRESSE";

            public const string LIGNE2_ADRESSE_tag = "LIGNE2_ADRESSE";
            public const string LIGNE1_ADRESSE_tag = "LIGNE1_ADRESSE";
            public const string LIGNE3_ADRESSE_tag = "LIGNE3_ADRESSE";
            public const string LIGNE4_ADRESSE_tag = "LIGNE4_ADRESSE";
            public const string COMMUNE_ETRANGERE_tag = "COMMUNE_ETRANGERE";
            public const string CODE_POSTAL_tag = "CODE_POSTAL";
            public const string CODE_PAYS_tag = "CODE_PAYS";
            public const string CODE_COMMUNE_INSEE_tag = "CODE_COMMUNE_INSEE";
            public const string LL_PAYS_tag = "LL_PAYS";
            public const string CODE_DEPARTEMENT_tag = "CODE_DEPARTEMENT";
            public const string LIBELLE_POSTAL_tag = "LIBELLE_POSTAL";
        }

        string fileName;



        public TRAAFile(string filename)
        {
            fileName = filename;
        }

        public IEnumerator<TRAAUser> GetEnumerator()
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

        private IEnumerator<TRAAUser> Enum(XmlDocument xmlFile)
        {
            Dictionary<string, TRAAUser.TAdresse> adresses = new Dictionary<string, TRAAUser.TAdresse>();

            if (xmlFile.DocumentElement.Name != PN.SCONET_tag) throw new Exception();
            XmlNodeList adressesNode = xmlFile.SelectNodes("/" + PN.SCONET_tag + "/" + PN.DONNEES_tag + "/" + PN.ADRESSES_tag + "/" + PN.ADRESSE_tag);
            XmlNode t;
            foreach (XmlNode a in adressesNode)
            {
                TRAAUser.TAdresse adr = new TRAAUser.TAdresse();
                var id = a.Attributes[PN.PERSONNE_ID_tag].Value;
                adr.CODE_COMMUNE_INSEE = (t = a.SelectSingleNode(PN.CODE_COMMUNE_INSEE_tag)) == null ? "" : t.InnerText.Trim();
                adr.CODE_DEPARTEMENT = (t = a.SelectSingleNode(PN.CODE_DEPARTEMENT_tag)) == null ? "" : t.InnerText.Trim();
                adr.CODE_PAYS = (t = a.SelectSingleNode(PN.CODE_PAYS_tag)) == null ? "" : t.InnerText.Trim();
                adr.CODE_POSTAL = (t = a.SelectSingleNode(PN.CODE_POSTAL_tag)) == null ? "" : t.InnerText.Trim();
                adr.COMMUNE_ETRANGERE = (t = a.SelectSingleNode(PN.COMMUNE_ETRANGERE_tag)) == null ? "" : t.InnerText.Trim();
                adr.LIBELLE_POSTAL = (t = a.SelectSingleNode(PN.LIBELLE_POSTAL_tag)) == null ? "" : t.InnerText.Trim();
                adr.LIGNE1_ADRESSE = (t = a.SelectSingleNode(PN.LIGNE1_ADRESSE_tag)) == null ? "" : t.InnerText.Trim();
                adr.LIGNE2_ADRESSE = (t = a.SelectSingleNode(PN.LIGNE2_ADRESSE_tag)) == null ? "" : t.InnerText.Trim();
                adr.LIGNE3_ADRESSE = (t = a.SelectSingleNode(PN.LIGNE3_ADRESSE_tag)) == null ? "" : t.InnerText.Trim();
                adr.LIGNE4_ADRESSE = (t = a.SelectSingleNode(PN.LIGNE4_ADRESSE_tag)) == null ? "" : t.InnerText.Trim();
                adr.LL_PAYS = (t = a.SelectSingleNode(PN.LL_PAYS_tag)) == null ? "" : t.InnerText.Trim();
                adresses.Add(id, adr);
            }

            Dictionary<string, HashSet<string>> joined = new Dictionary<string, HashSet<string>>();
            XmlNodeList JoinedNode = xmlFile.SelectNodes("/" + PN.SCONET_tag + "/" + PN.DONNEES_tag + "/" + PN.RESPONSABLES_tag + "/" + PN.RESPONSABLE_tag);
            foreach (XmlNode i in JoinedNode)
            {
                string ei = (t = i.SelectSingleNode(PN.ELEVE_ID_tag)) == null ? "" : t.InnerText.Trim();
                string pi = (t = i.SelectSingleNode(PN.PERSONNE_ID_tag)) == null ? "" : t.InnerText.Trim();
                HashSet<string> l;
                if (! joined.TryGetValue(pi, out l))
                {
                    l = new HashSet<string>();
                    joined.Add(pi, l);
                }
                l.Add(ei);
            }

            XmlNodeList PersonnesNode = xmlFile.SelectNodes("/" + PN.SCONET_tag + "/" + PN.DONNEES_tag + "/" + PN.PERSONNES_tag + "/" + PN.PERSONNE_tag);

            foreach (XmlNode i in PersonnesNode)
            {
                string pi = i.Attributes[PN.PERSONNE_ID_tag].Value;
                TRAAUser u = new TRAAUser();
                if (adresses.TryGetValue(pi, out TRAAUser.TAdresse ad))
                {
                    u.adresse = ad;
                }
                if (joined.TryGetValue(pi, out HashSet<string> l))
                {
                    u.childsIds = l;
                }
                u.Nom = (t = i.SelectSingleNode(PN.NOM_tag)) == null ? "" : t.InnerText.Trim();
                u.Prenom = (t = i.SelectSingleNode(PN.PRENOM_tag)) == null ? "" : t.InnerText.Trim();
                u.LL_CIVILITE  = (t = i.SelectSingleNode(PN.LL_CIVILITE_tag)) == null ? "" : t.InnerText.Trim();
                u.LC_CIVILITE = (t = i.SelectSingleNode(PN.LC_CIVILITE_tag)) == null ? "" : t.InnerText.Trim();
                yield return u;
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

    }



}
