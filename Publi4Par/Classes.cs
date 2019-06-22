using System;
using System.Collections.Generic;

namespace Publi4Par
{
    /// <summary>
    /// Représente un couple login, mdp
    /// </summary>
    public class TIds
    {
        public const string NoId = "#####";
        public const string Bull = "&bull;&bull;&bull;&bull;&bull;";
        public string id;
        public string pw;
        public string pwOrBull { get { return pw ?? Bull; } }
    }

    /// <summary>
    /// Représente un utilisateur avec 2 couples d'identifiants
    /// </summary>
    public abstract class TUser : IComparable<TUser>
    {
        public bool coche = false;
        public string nom;
        public string prenom;
        public string UID;
        public string UID2;
        public TIds Ids1;
        public TIds Ids2;
        public DateTime DateNaiss;
        public int CompareTo(TUser other)
        {
            if (other == null) return 1;
            int comp = nom.CompareTo(other.nom);
            if (comp == 0) return prenom.CompareTo(other.prenom);
            return comp;
        }
    }


}