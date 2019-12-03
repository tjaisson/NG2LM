using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Classes
{
    public class MyHashSet : HashSet<string>
    {
        public string Getvalue(string s)
        {
            if (TryGetValue(s, out string v))
            {
                return v;
            }
            else
            {
                Add(s);
                return s;
            }
        }
    }


    public class EducCsvReader : IEnumerable<EducCsvReader.TEducUser>
    {
        private MyHashSet GroupNames;
        private MyHashSet DivisionNames;
        private int[] positions;

        public abstract class TEducUser : TUser
        {
            public Stack<string> Groups = new Stack<string>();
        }

        private class TInnerEducUser : TEducUser
        {
            public TInnerEducUser(EducCsvReader r, string[] record)
            {
                nom = record[r.positions[(int)fields.N]].Trim();
                prenom = record[r.positions[(int)fields.P]].Trim();
                if (!DateTime.TryParse(record[r.positions[(int)fields.D]], out DateNaiss)) DateNaiss = DateTime.MinValue;
                string div = r.DivisionNames.Getvalue(record[r.positions[(int)fields.cl]].Trim());
                division = r.DivisionNames.Getvalue(record[r.positions[(int)fields.cl]].Trim());
                string grpsStr = record[r.positions[(int)fields.gr]].Trim();
                string[] grps = grpsStr.Split('$');
                foreach (var g in grps)
                {
                    if (g.Length <= 8)
                    {
                        string gg = r.GroupNames.Getvalue(g);
                        Groups.Push(gg);
                    }
                }
            }
        }

        //Login;Mot de passe;Profil;Nom;Prénom;Date de naissance;Sexe;Civilité;Classes;Groupes;Matieres;Mail

        enum fields { login, N, P, D, prf, cl, gr };
        static string[] Tags = { "login", "nom", "prenom", "date de naissance", "profil", "classes", "groupes" };
        const string elevPrf = "eleve";
        private string FF;
        public EducCsvReader(string F)
        {
            FF = F;
        }


        public IEnumerator<TEducUser> GetEnumerator()
        {
            GroupNames = new MyHashSet();
            DivisionNames = new MyHashSet();
            for (int j = 0; j < Tags.Length; j++) { Tags[j] = stringManip.simplifyName(Tags[j]); }

            positions = new int[Tags.Length];
            for (int i = 0; i < positions.Length; i++) positions[i] = -1;
            using (TextFieldParser TFP = new TextFieldParser(FF, Encoding.GetEncoding(28605))) // iso-8859-15
            {
                TFP.TextFieldType = FieldType.Delimited;
                TFP.SetDelimiters(";");
                TFP.HasFieldsEnclosedInQuotes = true;
                if (!TFP.EndOfData)
                {
                    string[] record = TFP.ReadFields();
                    for (int i = 0; i < record.Length; i++)
                    {
                        string f = stringManip.simplifyName(record[i].ToLower());
                        for (int j = 0; j < Tags.Length; j++) if (f == Tags[j]) positions[j] = i;
                    }
                    for (int j = 0; j < Tags.Length; j++) if (positions[j] < 0) yield break;
                    string elevPrfSimp = stringManip.simplifyName(elevPrf);
                    while (!TFP.EndOfData)
                    {
                        record = TFP.ReadFields();
                        if (stringManip.simplifyName(record[positions[(int)fields.prf]].Trim()) == elevPrfSimp)
                        {
                            if (!string.IsNullOrEmpty(record[positions[(int)fields.cl]].Trim()))
                            {
                                yield return new TInnerEducUser(this, record);
                            }
                        }
                    }
                }
            }
            GroupNames = null;
            DivisionNames = null;
            positions = null;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
