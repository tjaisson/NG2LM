using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.VisualBasic.FileIO;

namespace Lilie2LaboMep
{

    /// <summary>
    /// Représente une classe issue du fichier Lilie
    /// </summary>
    class CsvClasse
    {
        public CsvClasse(string N)
        {
            Eleves = new List<CsvUser>();
            Nom_Classe = N;
        }
        public string Nom_Classe;
        public List<CsvUser> Eleves;
        public override string ToString()
        {
            return Nom_Classe;
        }

    }


    /// <summary>
    /// Collection des classes lues dans le csv
    /// </summary>
    class CsvClassesList : SortedList<string, CsvClasse>
    {
        /// <summary>
        /// retourne la classe et la crée au besoin
        /// </summary>
        /// <param name="Nom_Classe">Le nom de la classe à rechercher ou à créer</param>
        /// <returns>l'objet CsvClasse existant ou créé lors de l'appel</returns>
        public CsvClasse GetOrCreate(string Nom_Classe)
        {
            string upN = Nom_Classe.ToUpper();
            CsvClasse Classe;
            if(! TryGetValue(upN, out Classe))
            {
                Classe = new CsvClasse(Nom_Classe);
                Add(upN, Classe);
            }
            return Classe;
        }
    }


    /// <summary>
    /// Représente un élève issu du fichier csv Lilie
    /// </summary>
    class CsvUser
    {
        public string Nom;
        public string Prenom;
        public string Id;
        public CsvClasse Classe;
        public string Nom_Classe
        { get { return Classe.Nom_Classe; } }
    }


    /// <summary>
    /// Permet de lire le fichier csv issu de Lilie
    /// </summary>
    class CsvReader
    {
        public CsvClassesList Liste_Classes;
        string _FN;

        /// <summary>
        /// Crée un lecteur de fichier csv.
        /// </summary>
        /// <param name="FN">Nom du fichier CSV Lilie.</param>
        public CsvReader(String FN)
        {
            _FN = FN;
            Liste_Classes = new CsvClassesList();
        }

        /// <summary>
        /// Effectue la lecture du fichier csv qui a été transmis lors de l'appel du constructeur.
        /// </summary>
        /// <returns>true si le fichier est au bon format, false sinon.</returns>
        public Boolean read()
        {
            Liste_Classes.Clear();
            int i;
            using (TextFieldParser FP = new TextFieldParser(_FN, System.Text.Encoding.GetEncoding("iso-8859-1")))
            {
                FP.HasFieldsEnclosedInQuotes = true;
                FP.TextFieldType = FieldType.Delimited;
                FP.Delimiters = new string[] { ";" };

                string[] fields;
                const string Id_head = "Identifiant";
                const string Nom_head = "Nom";
                const string Prenom_head = "Prénom";
                const string Classe_head = "Classe";

                int Nom_Pos = -1;
                int Prenom_Pos = -1;
                int Id_Pos = -1;
                int Classe_Pos = -1;

                if (FP.EndOfData)
                    return false;

                i = 0;
                int max = -1;
                fields = FP.ReadFields();
                foreach (var f in fields)
                {
                    string field = f.Trim();
                    if (field == Id_head) { Id_Pos = i; max = i; }
                    else if (field == Nom_head) { Nom_Pos = i; max = i; }
                    else if (field == Prenom_head) { Prenom_Pos = i; max = i; }
                    else if (field == Classe_head) { Classe_Pos = i; max = i; }
                    i++;
                }

                if ((Nom_Pos == -1) || (Prenom_Pos == -1) || (Id_Pos == -1) || (Classe_Pos == -1))
                    return false;

                i = 0;
                while (!FP.EndOfData)
                {
                    fields = FP.ReadFields();
                    if (fields.Length > max)
                    {
                        CsvUser u = new CsvUser();
                        u.Nom = fields[Nom_Pos].Trim();
                        u.Prenom = fields[Prenom_Pos].Trim();
                        u.Classe = Liste_Classes.GetOrCreate(fields[Classe_Pos].Trim());
                        u.Classe.Eleves.Add(u);
                        u.Id = "UT" + fields[Id_Pos].Trim();
                        i++;
                    }
                }
            }
            return (i > 0);
        }
    }
}
