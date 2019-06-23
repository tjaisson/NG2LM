using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic.FileIO;

namespace Publi4Par
{
    /// <summary>
    ///  Classe qui permet de lire un fichier csv (encodé iso-8859-1) ayant les champs suivants :
    /// "login";"pwd";"nom";"prenom";"datenaiss"
    /// Le champ "datenaiss" est optionnel. S'il est manquant, la date de naissance sera DateTime.MinValue
    /// </summary>
    public class TCsvFile : IEnumerable<TImportedUser>
    {
        private string _FN;

        public TCsvFile(string f)
        {
            _FN = f;
        }

        public IEnumerator<TImportedUser> GetEnumerator()
        {
            int i;
            using (TextFieldParser FP = new TextFieldParser(_FN, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                FP.HasFieldsEnclosedInQuotes = true;
                FP.TextFieldType = FieldType.Delimited;
                FP.Delimiters = new string[] { ";" };

                string[] fields;
                const string Nom_head = "nom";
                const string Prenom_head = "prenom";
                const string Date_head = "datenaiss";
                const string Group_head = "groupe";

                int Nom_Pos = -1;
                int Prenom_Pos = -1;
                int Date_Pos = -1;
                int Group_Pos = -1;

                if (FP.EndOfData)
                    yield break;

                i = 0;
                int max = -1;
                fields = FP.ReadFields();
                foreach (var f in fields)
                {
                    string field = f.Trim();
                    if (field == Nom_head) { Nom_Pos = i; max = i; }
                    else if (field == Prenom_head) { Prenom_Pos = i; max = i; }
                    else if (field == Date_head) { Date_Pos = i; max = i; }
                    else if (field == Group_head) { Group_Pos = i; max = i; }
                    i++;
                }

                if ((Nom_Pos == -1) || (Prenom_Pos == -1) || (Group_Pos == -1))
                    yield break;

                while (!FP.EndOfData)
                {
                    fields = FP.ReadFields();
                    if (fields.Length > max)
                    {
                        TImportedUser u = new TImportedUser();
                        u.Nom = fields[Nom_Pos].Trim();
                        u.Prenom = fields[Prenom_Pos].Trim();
                        u.Groupe = fields[Group_Pos].Trim();
                        u.SimpGroupe = stringManip.simplifyGroupName(u.Groupe);
                        yield return u;
                    }
                }
            }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }




    public class TCsvUser
    {
        public string profil;
        public string nom;
        public string prenom;
        public string groupe;
        public string dateNaiss;
        public string login;
        public string pwd;
        public bool hasNPL()
        {
            if ((nom == null) || (nom == "")) return false;
            if ((prenom == null) || (prenom == "")) return false;
            if ((login == null) || (login == "")) return false;
            return true;
        }
        public bool hasNPDL()
        {
            if ((nom == null) || (nom == "")) return false;
            if ((prenom == null) || (prenom == "")) return false;
            if ((dateNaiss == null) || (dateNaiss == "")) return false;
            if ((login == null) || (login == "")) return false;
            return true;
        }
        public bool hasNPD()
        {
            if ((nom == null) || (nom == "")) return false;
            if ((prenom == null) || (prenom == "")) return false;
            if ((dateNaiss == null) || (dateNaiss == "")) return false;
            return true;
        }
    }

    /// <summary>
    ///  Classe qui permet de lire un fichier csv (encodé iso-8859-1) ayant les champs suivants :
    /// "login";"pwd";"nom";"prenom";"datenaiss"
    /// </summary>
    public class TCsvFile_old
    {
        private string _FN;

        public TCsvFile_old(string f)
        {
            _FN = f;
        }

        public IEnumerable<TCsvUser> AllUsers()
        {
            int i;
            using (TextFieldParser FP = new TextFieldParser(_FN, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                FP.HasFieldsEnclosedInQuotes = true;
                FP.TextFieldType = FieldType.Delimited;
                FP.Delimiters = new string[] { ";" };

                string[] fields;
                const string Id_head = "login";
                const string Nom_head = "nom";
                const string Prenom_head = "prenom";
                const string Date_head = "datenaiss";
                const string PW_head = "pwd";

                int Nom_Pos = -1;
                int Prenom_Pos = -1;
                int Id_Pos = -1;
                int Date_Pos = -1;
                int PW_Pos = -1;

                if (FP.EndOfData)
                    yield break;

                i = 0;
                int max = -1;
                fields = FP.ReadFields();
                foreach (var f in fields)
                {
                    string field = f.Trim();
                    if (field == Id_head) { Id_Pos = i; max = i; }
                    else if (field == Nom_head) { Nom_Pos = i; max = i; }
                    else if (field == Prenom_head) { Prenom_Pos = i; max = i; }
                    else if (field == Date_head) { Date_Pos = i; max = i; }
                    else if (field == PW_head) { PW_Pos = i; max = i; }
                    i++;
                }

                if ((Nom_Pos == -1) || (Prenom_Pos == -1) || (Id_Pos == -1)
                    || (Date_Pos == -1) || (PW_Pos == -1))
                    yield break;

                while (!FP.EndOfData)
                {
                    fields = FP.ReadFields();
                    if (fields.Length > max)
                    {
                        TCsvUser u = new TCsvUser();
                        u.nom = fields[Nom_Pos].Trim();
                        u.prenom = fields[Prenom_Pos].Trim();
                        u.dateNaiss = fields[Date_Pos].Trim();
                        u.pwd = fields[PW_Pos].Trim();
                        u.login = fields[Id_Pos].Trim().ToLower();
                        yield return u;
                    }
                }
            }
        }
    }

    /// <summary>
    /// Classe qui permet de lire le fichier csv issu de Educ Horus
    /// </summary>
    class TCsvFileEduc
    {
        string _FN;
        public TCsvFileEduc(String FN)
        {
            _FN = FN;
        }

        public IEnumerable<TCsvUser> AllUsers()
        {
            int i;
            using (TextFieldParser FP = new TextFieldParser(_FN, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                FP.HasFieldsEnclosedInQuotes = true;
                FP.TextFieldType = FieldType.Delimited;
                FP.Delimiters = new string[] { ";" };

                //Login;Mot de passe;Profil;Nom;Prénom;Date de naissance;Sexe;Civilité;Classes;Groupes;Matieres;Mail
                // "-- modifié --"
                string[] fields;
                const string Id_head = "Login";
                const string Nom_head = "Nom";
                const string Prenom_head = "Prénom";
                const string Date_head = "Date de naissance";
                const string PW_head = "Mot de passe";
                const string Profil_head = "Profil";
                const string Classe_head = "Classes";

                int Nom_Pos = -1;
                int Prenom_Pos = -1;
                int Id_Pos = -1;
                int Date_Pos = -1;
                int PW_Pos = -1;
                int Profil_Pos = -1;
                int Classe_Pos = -1;

                if (FP.EndOfData)
                    yield break;

                i = 0;
                int max = -1;
                fields = FP.ReadFields();
                foreach (var f in fields)
                {
                    string field = f.Trim();
                    if (field == Id_head) { Id_Pos = i; max = i; }
                    else if (field == Nom_head) { Nom_Pos = i; max = i; }
                    else if (field == Prenom_head) { Prenom_Pos = i; max = i; }
                    else if (field == Date_head) { Date_Pos = i; max = i; }
                    else if (field == PW_head) { PW_Pos = i; max = i; }
                    else if (field == Profil_head) { Profil_Pos = i; max = i; }
                    else if (field == Classe_head) { Classe_Pos = i; max = i; }
                    i++;
                }

                if ((Nom_Pos == -1) || (Prenom_Pos == -1) || (Id_Pos == -1) || (Classe_Pos == -1)
                    || (Date_Pos == -1) || (PW_Pos == -1) || (Profil_Pos == -1))
                    yield break;

                while (!FP.EndOfData)
                {
                    fields = FP.ReadFields();
                    if (fields.Length > max)
                    {
                        TCsvUser u = new TCsvUser();
                        u.nom = fields[Nom_Pos].Trim();
                        u.prenom = fields[Prenom_Pos].Trim();
                        u.dateNaiss = fields[Date_Pos].Trim();
                        u.pwd = fields[PW_Pos].Trim();
                        u.login = fields[Id_Pos].Trim().ToLower();
                        u.profil = fields[Profil_Pos].Trim();
                        u.groupe = fields[Classe_Pos].Trim();
                        yield return u;
                    }
                }
            }
        }
    }


    /// <summary>
    /// Classe qui permet de lire le fichier csv issu de Entcore
    /// "Id";"Id Siecle";"Type";"Nom";"Prénom";"Login";"Code d'activation";"Fonction(s)";"Structure(s)";"Classe(s)";"Enfant(s)";"Parent(s)"
    /// </summary>
    public class TCsvFileEntcore : IEnumerable<TImportedUser>
    {
        private string _FN;

        public TCsvFileEntcore(string f)
        {
            _FN = f;
        }

        enum fields { login, pw, N, P, id, prf, id2 };
        //"Id";"Id Siecle";"Type";"Nom";"Prénom";"Login";"Code d'activation";"Fonction(s)";"Structure(s)";"Classe(s)";"Enfant(s)";"Parent(s)"
        static readonly string[] Tags = { "Login", "Code d'activation", "Nom", "Prénom", "Id Siecle", "Type", "Id" };
        const string elevPrf = "Elève";

        private TImportedUser BuildIU(string[] record, int[] positions)
        {
            TImportedUser u = new TImportedUser();
            u.Nom = record[positions[(int)fields.N]].Trim();
            u.Prenom = record[positions[(int)fields.P]].Trim();
            u.UID = record[positions[(int)fields.id]].Trim();
            u.UID2 = record[positions[(int)fields.id2]].Trim();
            u.Login = record[positions[(int)fields.login]].Trim();
            u.PW = record[positions[(int)fields.pw]].Trim();
            return u;
        }

        public IEnumerator<TImportedUser> GetEnumerator()
        {
            int[] positions = new int[Tags.Length];
            for (int i = 0; i < positions.Length; i++) positions[i] = -1;

            using (TextFieldParser TFP = new TextFieldParser(_FN, new UTF8Encoding(true)))
            {
                TFP.HasFieldsEnclosedInQuotes = true;
                TFP.TextFieldType = FieldType.Delimited;
                TFP.Delimiters = new string[] { ";" };

                //"Id";"Id Siecle";"Type";"Nom";"Prénom";"Login";"Code d'activation";"Fonction(s)";"Structure(s)";"Classe(s)";"Enfant(s)";"Parent(s)"
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
                            yield return BuildIU(record, positions);
                        }
                    }
                }
            }
        }


        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
