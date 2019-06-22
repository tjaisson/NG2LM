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

    /// <summary>
    /// Classe qui représente un utilisateur à importer
    /// </summary>
    public class TRAAUser
    {
        public class TAdresse
        {
            public string LIGNE2_ADRESSE;
            public string LIGNE1_ADRESSE;
            public string LIGNE3_ADRESSE;
            public string LIGNE4_ADRESSE;
            public string COMMUNE_ETRANGERE;
            public string CODE_POSTAL;
            public string CODE_PAYS;
            public string CODE_COMMUNE_INSEE;
            public string LL_PAYS;
            public string CODE_DEPARTEMENT;
            public string LIBELLE_POSTAL;

        }

        public string Nom;
        public string Prenom;
        public string LC_CIVILITE;
        public string LL_CIVILITE;
        public TAdresse adresse;
        public HashSet<string> childsIds;
    }


}
