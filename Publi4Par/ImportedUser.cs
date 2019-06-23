using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Publi4Par
{


    /// <summary>
    /// Classe qui représente un utilisateur à importer
    /// </summary>
    public class TImportedUser
    {
        public string Nom;
        public string Prenom;
        public string Groupe;
        public string SimpGroupe;
        public string UID;
        public string UID2;
        public string Login;
        public string PW;
        public bool hasUID
        {
            get
            {
                if ((UID == null) || (UID == "")) return false;
                return true;
            }
        }
        public bool hasUID2
        {
            get
            {
                if ((UID2 == null) || (UID2 == "")) return false;
                return true;
            }
        }
        public bool hasNP
        {
            get
            {
                if ((Nom == null) || (Nom == "")) return false;
                return ((Prenom != null) && (Prenom != ""));
            }
        }
        public bool hasNPG
        {
            get
            {
                if ((Nom == null) || (Nom == "")) return false;
                if ((Prenom == null) || (Prenom == "")) return false;
                if ((Groupe == null) || (Groupe == "")) return false;
                return true;
            }
        }
        public bool hasLogin { get { return (Login != null) && (Login != ""); } }
    }



}
