using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace Publi4Par
{
    public struct NP
    {
        public string N, P;
        public NP(string n, string p)
        {
            this.N = stringManip.simplifyName(n);
            this.P = stringManip.simplifyName(p);
        }
    }



    /// <summary>
    /// Classe statique qui offre des méthodes pour manipuler les chaines de caractères
    /// Pour notamment tenir compte des caractères spéciaux.
    /// </summary>
    public static class stringManip
    {
        private static bool isgoodNb(char c, out char[] n)
        {

            if ((c >= '0') && (c <= '9'))
            {
                n = new char[1] { c };
                return true;
            }
            n = null;
            return false;
        }
        private static bool isgoodLow(char c, out char[] n)
        {

            if ((c >= 'a') && (c <= 'z'))
            {
                n = new char[1] { c };
                return true;
            }
            else if ((c == 'á') || (c == 'â') || (c == 'ã') || (c == 'ä') || (c == 'à'))
            {
                n = new char[1] { 'a' };
                return true;
            }
            else if (c == 'æ')  //alt 0230
            {
                n = new char[2] { 'a', 'e' };
                return true;
            }
            else if (c == 'ç')  //alt 0231
            {
                n = new char[1] { 'c' };
                return true;
            }
            else if ((c == 'è') || (c == 'é') || (c == 'ê') || (c == 'ë'))   // alt 0235
            {
                n = new char[1] { 'e' };
                return true;
            }
            else if ((c == 'ì') || (c == 'í') || (c == 'î') || (c == 'ï'))   // alt 0239
            {
                n = new char[1] { 'i' };
                return true;
            }
            else if ((c == 'ð') || (c == 'ò') || (c == 'ó') || (c == 'ô') || (c == 'õ') || (c == 'ö'))   // alt 0246
            {
                n = new char[1] { 'o' };
                return true;
            }
            else if (c == 'ñ')   // alt 0241
            {
                n = new char[1] { 'n' };
                return true;
            }
            else if ((c == 'ù') || (c == 'ú') || (c == 'û') || (c == 'ü'))   // alt 0252
            {
                n = new char[1] { 'u' };
                return true;
            }
            else if (c == 'œ')  //??
            {
                n = new char[2] { 'o', 'e' };
                return true;
            }
            else
            {
                n = null;
                return false;
            }
        }
        private static bool isgoodUp(char c, out char[] n)
        {

            if ((c >= 'A') && (c <= 'Z'))
            {
                n = new char[1] { c };
                return true;
            }
            else if ((c == 'Á') || (c == 'Â') || (c == 'Ã') || (c == 'Ä') || (c == 'À'))
            {
                n = new char[1] { 'A' };
                return true;
            }
            else if (c == 'Æ')  //alt 0198
            {
                n = new char[2] { 'A', 'E' };
                return true;
            }
            else if (c == 'Ç')  //alt 0199
            {
                n = new char[1] { 'C' };
                return true;
            }
            else if ((c == 'È') || (c == 'É') || (c == 'Ê') || (c == 'Ë'))   // alt 0203
            {
                n = new char[1] { 'E' };
                return true;
            }
            else if ((c == 'Ì') || (c == 'Í') || (c == 'Î') || (c == 'Ï'))   // alt 0207
            {
                n = new char[1] { 'I' };
                return true;
            }
            else if ((c == 'Ò') || (c == 'Ó') || (c == 'Ô') || (c == 'Õ') || (c == 'Ö'))   // alt 0214
            {
                n = new char[1] { 'O' };
                return true;
            }
            else if (c == 'Ñ')   // alt 0209
            {
                n = new char[1] { 'N' };
                return true;
            }
            else if ((c == 'Ù') || (c == 'Ú') || (c == 'Û') || (c == 'Ü'))   // alt 0220
            {
                n = new char[1] { 'U' };
                return true;
            }
            else if (c == 'Œ')  //??
            {
                n = new char[2] { 'O', 'E' };
                return true;
            }
            else
            {
                n = null;
                return false;
            }
        }
        public static string getInitial(string s)
        {
            s = s.ToLowerInvariant();
            char[] cs = s.ToCharArray();
            for (int i = 0; i < cs.Length; i++)
            {
                char[] n;
                if (isgoodLow(cs[i], out n))
                {
                    return n[0].ToString();
                }
            }
            return ".";
        }

        /// <summary>
        /// transforme une chaine de caractères en minuscule et remplaçe tous les caractères spéciaux
        /// les espace sont remplacés par des tirets
        /// </summary>
        /// <param name="s">La chaine à simplifier</param>
        /// <returns>la chaine transformée</returns>
        public static string simplifyName(string s)
        {
            s = s.ToLowerInvariant();
            bool skip = true;
            bool started = false;
            char[] cs = s.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cs.Length; i++)
            {
                char[] n;
                if (isgoodLow(cs[i], out n))
                {
                    if (started && skip)
                        sb.Append('-');
                    sb.Append(n);
                    started = true;
                    skip = false;
                }
                else
                {
                    skip = true;
                }
            }
            return sb.ToString();
        }

        public static string simplifyGroupName(string s)
        {
            s = s.ToUpperInvariant();
            bool skip = true;
            bool started = false;
            char[] cs = s.ToCharArray();
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < cs.Length; i++)
            {
                char[] n;
                if (isgoodUp(cs[i], out n) || isgoodNb(cs[i], out n))
                {
                    if (started && skip)
                        sb.Append('-');
                    sb.Append(n);
                    started = true;
                    skip = false;
                }
                else
                {
                    skip = true;
                }
            }
            return sb.ToString();
        }

        public static string simplifyDate(string s)
        {
            return s.Replace("/", "");
        }
    }

}