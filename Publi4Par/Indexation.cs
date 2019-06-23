using System;
using System.Collections.Generic;

namespace Publi4Par
{
    public class Checkable<T>
    {
        public Checkable(T Object) { this.FObject = Object; }
        public bool Checked = false;
        private T FObject;
        public T Object { get { return FObject; } }
    }

    /// <summary>
    /// Crée un index par NPD
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TIndexByNPD<T>
    {
        public struct NPD
        {
            public DateTime D;
            public string N, P;
            public NPD(string n, string p, DateTime d)
            {
                this.N = n;
                this.P = p;
                this.D = d;
            }
        }

        protected Dictionary<NPD, T> FIndex = new Dictionary<NPD, T>();

        public int Count { get { return FIndex.Count; } }

        /// <summary>
        /// Ajoute une entrée dans l'index
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <param name="D">Date</param>
        /// <param name="Value">la donnée à indexer</param>
        public void Add(string N, string P, DateTime D, T Value)
        {
            N = stringManip.simplifyName(N);
            P = stringManip.simplifyName(P);
            FIndex[new NPD(N, P, D)] = Value;
        }

        /// <summary>
        /// essaye de récupérer une donnée et la retire si elle est trouvée
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <param name="D">Date</param>
        /// <param name="Value">La donnée récupérée</param>
        /// <returns></returns>
        public bool TryExtract(string N, string P, DateTime D, out T Value)
        {
            NPD npd = new NPD(N, P, D);
            if(FIndex.TryGetValue(npd, out Value))
            {
                FIndex.Remove(npd);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enlève une entrée de l'index
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <param name="D">Date</param>
        public void Remove(string N, string P, DateTime D)
        {
            N = stringManip.simplifyName(N);
            P = stringManip.simplifyName(P);
            FIndex.Remove(new NPD(N, P, D));
        }

        /// <summary>
        /// récupère une donnée dans l'index
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <param name="D">Date</param>
        /// <returns>la donnée ou default(T) si pas de donnée</returns>
        public T this[string N, string P, DateTime D]
        {
            get
            {
                N = stringManip.simplifyName(N);
                P = stringManip.simplifyName(P);
                T val;
                if (FIndex.TryGetValue(new NPD(N, P, D), out val))
                {
                    return val;
                }
                return default(T);
            }
        }

        public Dictionary<NPD, T> Index
        {
            get { return FIndex; }
        }
    }

    /// <summary>
    /// Crée un index par NP
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TIndexByNP<T>
    {
        public struct NP
        {
            public string N, P;
            public NP(string n, string p)
            {
                this.N = n;
                this.P = p;
            }
        }

        protected Dictionary<NP, T> FIndex = new Dictionary<NP, T>();

        public int Count { get { return FIndex.Count; } }

        /// <summary>
        /// Ajoute une entrée dans l'index
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <param name="Value">la donnée à indexer</param>
        public void Add(string N, string P, T Value)
        {
            N = stringManip.simplifyName(N);
            P = stringManip.simplifyName(P);
            FIndex[new NP(N, P)] = Value;
        }

        /// <summary>
        /// essaye de récupérer une donnée et la retire si elle est trouvée
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <param name="Value">La donnée récupérée</param>
        /// <returns></returns>
        public bool TryExtract(string N, string P, out T Value)
        {
            NP np = new NP(N, P);
            if (FIndex.TryGetValue(np, out Value))
            {
                FIndex.Remove(np);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enlève une entrée de l'index
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        public void Remove(string N, string P)
        {
            N = stringManip.simplifyName(N);
            P = stringManip.simplifyName(P);
            FIndex.Remove(new NP(N, P));
        }

        /// <summary>
        /// récupère une donnée dans l'index
        /// </summary>
        /// <param name="N">Nom</param>
        /// <param name="P">Prénom</param>
        /// <returns>la donnée ou default(T) si pas de donnée</returns>
        public T this[string N, string P]
        {
            get
            {
                N = stringManip.simplifyName(N);
                P = stringManip.simplifyName(P);
                T val;
                if (FIndex.TryGetValue(new NP(N, P), out val))
                {
                    return val;
                }
                return default(T);
            }
        }

        public Dictionary<NP, T> Index
        {
            get { return FIndex; }
        }
    }

    /// <summary>
    /// Crée un index par UID
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TIndexByUID<T>
    {
        protected Dictionary<string, T> FIndex = new Dictionary<string, T>();

        public int Count { get { return FIndex.Count; } }

        /// <summary>
        /// Ajoute une entrée dans l'index
        /// </summary>
        /// <param name="UID">UID</param>
        /// <param name="Value">la donnée à indexer</param>
        public void Add(string UID, T Value)
        {
            FIndex[UID] = Value;
        }

        /// <summary>
        /// essaye de récupérer une donnée et la retire si elle est trouvée
        /// </summary>
        /// <param name="UID">UID</param>
        /// <param name="Value">La donnée récupérée</param>
        /// <returns></returns>
        public bool TryExtract(string UID, out T Value)
        {
            if (FIndex.TryGetValue(UID, out Value))
            {
                FIndex.Remove(UID);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Enlève une entrée de l'index
        /// </summary>
        /// <param name="UID">UID</param>
        public void Remove(string UID)
        {
            FIndex.Remove(UID);
        }

        /// <summary>
        /// récupère une donnée dans l'index
        /// </summary>
        /// <param name="UID">UID</param>
        /// <returns>la donnée ou default(T) si pas de donnée</returns>
        public T this[string UID]
        {
            get
            {
                T val;
                if (FIndex.TryGetValue(UID, out val))
                {
                    return val;
                }
                return default(T);
            }
        }

        public Dictionary<string, T> Index
        {
            get { return FIndex; }
        }
    }


}
