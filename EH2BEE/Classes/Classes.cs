using System;
using System.Collections.Generic;

namespace Classes
{
    /// <summary>
    /// Représente un utilisateur avec 2 couples d'identifiants
    /// </summary>
    public abstract class TUser : IComparable<TUser>
    {
        public string nom;
        public string prenom;
        public DateTime DateNaiss;
        public string division;
        public int CompareTo(TUser other)
        {
            if (other == null) return 1;
            int comp = nom.CompareTo(other.nom);
            if (comp == 0) return prenom.CompareTo(other.prenom);
            return comp;
        }
    }
}