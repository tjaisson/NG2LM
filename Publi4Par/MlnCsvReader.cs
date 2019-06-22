using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Publi4Par
{
    public class MlnCsvReader
    {
        private class TInnerMlnUser : TUser
        {
            public TInnerMlnUser(string[] record, int[] positions)
            {
                Ids1 = new TIds();
                Ids1.id = record[positions[(int)fields.login]].Trim().ToLower();
                string tmp = record[positions[(int)fields.pw]].Trim();
                if (string.IsNullOrEmpty(tmp))
                {
                    Ids1.pw = null;
                }
                else
                {
                    Ids1.pw = tmp;
                }
                nom = record[positions[(int)fields.N]].Trim();
                prenom = record[positions[(int)fields.P]].Trim();
                UID2 = record[positions[(int)fields.id]].Trim();
            }
        }

        enum fields { login, pw, N, P, id, prf };
        //"Id";"Id Siecle";"Type";"Nom";"Prénom";"Login";"Code d'activation";"Fonction(s)";"Structure(s)";"Classe(s)";"Enfant(s)";"Parent(s)"
        static readonly string[] Tags = { "Login", "Code d'activation", "Nom", "Prénom", "Id Siecle", "Type" };
        const string elevPrf = "Elève";
        private string FF;
        public MlnCsvReader(string F)
        {
            FF = F;
        }

        public IEnumerable<TUser> Eleves()
        {
            int[] positions = new int[Tags.Length];
            for (int i = 0; i < positions.Length; i++) positions[i] = -1;
            using (TextFieldParser TFP = new TextFieldParser(FF, new UTF8Encoding(true))) // iso-8859-15
            {
                TFP.TextFieldType = FieldType.Delimited;
                TFP.SetDelimiters(";");
                TFP.HasFieldsEnclosedInQuotes = true;
                //Login;Mot de passe;Profil;Nom;Prénom;Date de naissance;Sexe;Civilité;Classes;Groupes;Matieres;Mail
                if (!TFP.EndOfData)
                {
                    string[] record = TFP.ReadFields();
                    for (int i = 0; i < record.Length; i++)
                    {
                        string f = record[i].ToLower();
                        for (int j = 0; j < Tags.Length; j++) if (f.Equals(Tags[j], StringComparison.CurrentCultureIgnoreCase)) positions[j] = i;
                    }
                    for (int j = 0; j < Tags.Length; j++) if (positions[j] < 0) yield break;
                    while (!TFP.EndOfData)
                    {
                        record = TFP.ReadFields();
                        if (record[positions[(int)fields.prf]].Trim() == elevPrf)
                        {
                            yield return new TInnerMlnUser(record, positions);
                        }
                    }
                }
            }
        }
    }


    public class MlnEPSReader
    {
        public class TEPSLine
        {
            public TEPSLine(string[] record, int[] positions)
            {
                Ids1 = new TIds();
                Ids1.id = record[positions[(int)fields.login]].Trim().ToLower();
                string tmp = record[positions[(int)fields.pw]].Trim();
                if (string.IsNullOrEmpty(tmp))
                {
                    Ids1.pw = null;
                }
                else
                {
                    Ids1.pw = tmp;
                }
                nom = record[positions[(int)fields.N]].Trim();
                prenom = record[positions[(int)fields.P]].Trim();
                UID2 = record[positions[(int)fields.id]].Trim();
            }
        }

        enum fields {uid, pw, N, P, id, prf };
        //Identifiant_ENT,Profil,nom,prenom,Date de naissance,Sexe,Classe,ID_Sconet_Eleve1,ID_Sconet_Eleve2,ID_Sconet_Eleve3,ID_Sconet_Eleve4,ID_Sconet_Eleve5
        static readonly string[] Tags = { "Identifiant_ENT", "Code d'activation", "Nom", "Prénom", "Id Siecle", "Type" };
        private string FF;
        public MlnEPSReader(string F)
        {
            FF = F;
        }

        public IEnumerable<TEPSLine> Lines()
        {
            int[] positions = new int[Tags.Length];
            for (int i = 0; i < positions.Length; i++) positions[i] = -1;
            using (TextFieldParser TFP = new TextFieldParser(FF, new UTF8Encoding(true))) // iso-8859-15
            {
                TFP.TextFieldType = FieldType.Delimited;
                TFP.SetDelimiters(";");
                TFP.HasFieldsEnclosedInQuotes = true;
                //Login;Mot de passe;Profil;Nom;Prénom;Date de naissance;Sexe;Civilité;Classes;Groupes;Matieres;Mail
                if (!TFP.EndOfData)
                {
                    string[] record = TFP.ReadFields();
                    for (int i = 0; i < record.Length; i++)
                    {
                        string f = record[i].ToLower();
                        for (int j = 0; j < Tags.Length; j++) if (f.Equals(Tags[j], StringComparison.CurrentCultureIgnoreCase)) positions[j] = i;
                    }
                    for (int j = 0; j < Tags.Length; j++) if (positions[j] < 0) yield break;
                    while (!TFP.EndOfData)
                    {
                        record = TFP.ReadFields();
                        if (record[positions[(int)fields.prf]].Trim() == elevPrf)
                        {
                            yield return new TEPSLine(record, positions);
                        }
                    }
                }
            }
        }
    }
}