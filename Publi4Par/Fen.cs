using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace Publi4Par
{
    public partial class Fen : Form
    {
        int step = 0;

        public Fen()
        {
            InitializeComponent();
            SaveLNK.Hide();
            EchecLNK.Hide();
            Memo.AppendText("Pour commencer, veuillez déposer ici le fichier ExportXML « ResponsablesAvecAdresses » (zip ou xml)\r\n");
            step = 0;
        }

        private void Fen_DragEnter(object sender, DragEventArgs e)
        {
            if ((step == 0 || step == 1) && (e.Data.GetDataPresent(DataFormats.FileDrop)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Fen_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
                ReadFile(files[0]);
            }
        }

        private void ReadFile(string fn)
        {
            switch (step)
            {
                case 0:
                    ReadRAA(fn);
                    break;
                case 1:
                    ReadMLN(fn);
                    break;
                default:
                    break;
            }
        }

        private Dictionary<NP, TRAAFile.TRAAUser> RAAUsers;
        private Dictionary<NP, Stack<TRAAFile.TRAAUser>> RAAUsersCol;

        private void ReadRAA(string fn)
        {
            RAAUsers = new Dictionary<NP, TRAAFile.TRAAUser>();
            RAAUsersCol = new Dictionary<NP, Stack<TRAAFile.TRAAUser>>();
            try
            {
                int i = 0;
                Memo.AppendText("Lecture fichier...\r\n");
                foreach (var p in new TRAAFile(fn))
                {
                    i++;
                    Stack<TRAAFile.TRAAUser> stck;
                    NP np = new NP(p.Nom, p.Prenom);
                    if(RAAUsersCol.TryGetValue(np, out stck))
                    {
                        stck.Push(p);
                    }
                    else if (RAAUsers.TryGetValue(np, out var pp))
                    {
                        RAAUsers.Remove(np);
                        stck = new Stack<TRAAFile.TRAAUser>();
                        stck.Push(p);
                        stck.Push(pp);
                        RAAUsersCol.Add(np, stck);
                    }
                    else
                    {
                        RAAUsers.Add(np, p);
                    }
                }
                if (i == 0)
                {
                    Memo.AppendText(string.Format("Fichier Vide...\r\n", i));
                    return;
                }
                Memo.AppendText(string.Format("Fichier lu, {0} personnes trouvées\r\n", i));
                i = RAAUsersCol.Count;
                if (i > 0)
                {
                    Memo.AppendText(string.Format("info : {0} cas d'homonymes Nom & Prénom détectés.\r\n", i));
                }
                Memo.AppendText("\r\nPour continuer, veuillez déposer ici le fichier export standard MonLycée.net (csv)\r\n");
                step = 1;
            }
            catch (Exception)
            {
                Memo.AppendText("Le fichier n'a pas le bon format\r\n");
            }
        }

        private Dictionary<NP, MlnCsvReader.TMlnUser> MLNUsers;
        private Dictionary<NP, Stack<MlnCsvReader.TMlnUser>> MLNUsersCol;
        private Dictionary<MlnCsvReader.TMlnUser, TRAAFile.TRAAUser> assoc;
        private Dictionary<MlnCsvReader.TMlnUser, Stack<TRAAFile.TRAAUser>> semiAssoc;
        private Stack<MlnCsvReader.TMlnUser> echec;
        private Dictionary<Stack<MlnCsvReader.TMlnUser>, TRAAFile.TRAAUser> assocCol;
        private Dictionary<Stack<MlnCsvReader.TMlnUser>, Stack<TRAAFile.TRAAUser>> semiAssocCol;
        private Stack<Stack<MlnCsvReader.TMlnUser>> echecCol;


        private void ReadMLN(string fn)
        {
            MLNUsers = new Dictionary<NP, MlnCsvReader.TMlnUser>();
            MLNUsersCol = new Dictionary<NP, Stack<MlnCsvReader.TMlnUser>>();
            try
            {
                int i = 0;
                Memo.AppendText("Lecture fichier...\r\n");
                foreach (var p in (new MlnCsvReader(fn)).Parents())
                {
                    i++;
                    Stack<MlnCsvReader.TMlnUser> stck;
                    NP np = new NP(p.N, p.P);
                    if (MLNUsersCol.TryGetValue(np, out stck))
                    {
                        stck.Push(p);
                    }
                    else if (MLNUsers.TryGetValue(np, out var pp))
                    {
                        MLNUsers.Remove(np);
                        stck = new Stack<MlnCsvReader.TMlnUser>();
                        stck.Push(p);
                        stck.Push(pp);
                        MLNUsersCol.Add(np, stck);
                    }
                    else
                    {
                        MLNUsers.Add(np, p);
                    }
                }
                if (i == 0)
                {
                    Memo.AppendText(string.Format("Fichier Vide...\r\n", i));
                    return;
                }
                Memo.AppendText(string.Format("Fichier lu, {0} personnes trouvées\r\n", i));
                i = MLNUsersCol.Count;
                if (i > 0)
                {
                    Memo.AppendText(string.Format("info : {0} cas d'homonymes Nom & Prénom détectés.\r\n", i));
                }
                step = 2;
            }
            catch (Exception)
            {
                Memo.AppendText("Le fichier n'a pas le bon format\r\n");
                return;
            }

            try
            {
                Memo.AppendText("\r\nMise en correspondance...\r\n");
                int a = 0;
                int sa = 0;
                int e = 0;

                assoc = new Dictionary<MlnCsvReader.TMlnUser, TRAAFile.TRAAUser>();
                semiAssoc = new Dictionary<MlnCsvReader.TMlnUser, Stack<TRAAFile.TRAAUser>>();
                echec = new Stack<MlnCsvReader.TMlnUser>();
                assocCol = new Dictionary<Stack<MlnCsvReader.TMlnUser>, TRAAFile.TRAAUser>();
                semiAssocCol = new Dictionary<Stack<MlnCsvReader.TMlnUser>, Stack<TRAAFile.TRAAUser>>();
                echecCol = new Stack<Stack<MlnCsvReader.TMlnUser>>();
                foreach (var kvp in MLNUsers)
                {
                    if (RAAUsers.TryGetValue(kvp.Key, out var u))
                    {
                        assoc.Add(kvp.Value, u);
                        a++;
                    }
                    else if (RAAUsersCol.TryGetValue(kvp.Key, out var uc))
                    {
                        semiAssoc.Add(kvp.Value, uc);
                        sa++;
                    }
                    else
                    {
                        echec.Push(kvp.Value);
                        e++;
                    }
                }
                foreach (var kvp in MLNUsersCol)
                {
                    int nb = kvp.Value.Count;
                    if (RAAUsers.TryGetValue(kvp.Key, out var u))
                    {
                        assocCol.Add(kvp.Value, u);
                        sa += nb;
                    }
                    else if (RAAUsersCol.TryGetValue(kvp.Key, out var uc))
                    {
                        semiAssocCol.Add(kvp.Value, uc);
                        sa += nb;
                    }
                    else
                    {
                        echecCol.Push(kvp.Value);
                        e += nb;
                    }
                }
                RAAUsers = null;
                RAAUsersCol = null;
                MLNUsers = null;
                MLNUsersCol = null;
                Memo.AppendText(string.Format("Terminé :\r\n{0} adresses trouvées\r\n", a));
                if (sa > 0 || e > 0)
                {
                    EchecLNK.Show();
                    if (sa > 0)
                        Memo.AppendText(string.Format("{0} cas d'ambiguïté (homonymes)\r\n", sa));
                    if (e > 0)
                        Memo.AppendText(string.Format("{0} adresses non trouvées\r\n", e));
                }
                step = 3;
                Memo.AppendText(string.Format("\r\nVous pouvez générer le fichier.\r\n", a));
                SaveLNK.Show();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private IEnumerable<string> HeaderRow()
        {
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.id];
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.N]; //u.N;
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.P];//u.P;
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.login];//u.login;
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.pw];//u.pw;
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.classes];//u.classes;
            yield return MlnCsvReader.Tags[(int)MlnCsvReader.fields.childs];//u.childs;
            yield return TRAAFile.PN.LC_CIVILITE_tag;
            yield return TRAAFile.PN.LIGNE1_ADRESSE_tag;
            yield return TRAAFile.PN.LIGNE2_ADRESSE_tag;
            yield return TRAAFile.PN.LIGNE3_ADRESSE_tag;
            yield return TRAAFile.PN.LIGNE4_ADRESSE_tag;
            yield return TRAAFile.PN.COMMUNE_ETRANGERE_tag;
            yield return TRAAFile.PN.CODE_POSTAL_tag;
            yield return TRAAFile.PN.CODE_PAYS_tag;
            yield return TRAAFile.PN.CODE_COMMUNE_INSEE_tag;
            yield return TRAAFile.PN.LL_PAYS_tag;
            yield return TRAAFile.PN.CODE_DEPARTEMENT_tag;
            yield return TRAAFile.PN.LIBELLE_POSTAL_tag;
        }

        private IEnumerable<string> AssocUserRow(MlnCsvReader.TMlnUser u, TRAAFile.TRAAUser uu)
        {
            yield return u.id;
            yield return u.N;
            yield return u.P;
            yield return u.login;
            yield return u.pw;
            yield return u.classes;
            yield return u.childs;
            yield return uu.LC_CIVILITE;
            yield return uu.adresse.LIGNE1_ADRESSE;
            yield return uu.adresse.LIGNE2_ADRESSE;
            yield return uu.adresse.LIGNE3_ADRESSE;
            yield return uu.adresse.LIGNE4_ADRESSE;
            yield return uu.adresse.COMMUNE_ETRANGERE;
            yield return uu.adresse.CODE_POSTAL;
            yield return uu.adresse.CODE_PAYS;
            yield return uu.adresse.CODE_COMMUNE_INSEE;
            yield return uu.adresse.LL_PAYS;
            yield return uu.adresse.CODE_DEPARTEMENT;
            yield return uu.adresse.LIBELLE_POSTAL;
        }

        private IEnumerable<string> MlnUserRow(MlnCsvReader.TMlnUser u)
        {
            yield return u.id;
            yield return u.N;
            yield return u.P;
            yield return u.login;
            yield return u.pw;
            yield return u.classes;
            yield return u.childs;
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return "";
        }

        private IEnumerable<string> RAAUserRow(TRAAFile.TRAAUser uu)
        {
            yield return "";
            yield return uu.Nom;
            yield return uu.Prenom;
            yield return "";
            yield return "";
            yield return "";
            yield return "";
            yield return uu.LC_CIVILITE;
            yield return uu.adresse.LIGNE1_ADRESSE;
            yield return uu.adresse.LIGNE2_ADRESSE;
            yield return uu.adresse.LIGNE3_ADRESSE;
            yield return uu.adresse.LIGNE4_ADRESSE;
            yield return uu.adresse.COMMUNE_ETRANGERE;
            yield return uu.adresse.CODE_POSTAL;
            yield return uu.adresse.CODE_PAYS;
            yield return uu.adresse.CODE_COMMUNE_INSEE;
            yield return uu.adresse.LL_PAYS;
            yield return uu.adresse.CODE_DEPARTEMENT;
            yield return uu.adresse.LIBELLE_POSTAL;
        }

        private IEnumerable<IEnumerable<string>> assocsLines()
        {
            yield return HeaderRow();
            foreach (var kvp in assoc)
            {
                yield return AssocUserRow(kvp.Key, kvp.Value);
            }
        }

        private IEnumerable<IEnumerable<string>> echecsLines()
        {
            yield return HeaderRow();
            foreach (var u in echec)
            {
                yield return MlnUserRow(u);
            }
            foreach (var stk in echecCol)
            {
                foreach (var u in stk)
                {
                    yield return MlnUserRow(u);
                }
            }
            foreach (var kvp in assocCol)
            {
                foreach (var u in kvp.Key)
                {
                    yield return MlnUserRow(u);
                }
                yield return RAAUserRow(kvp.Value);
            }
            foreach (var kvp in semiAssoc)
            {
                yield return MlnUserRow(kvp.Key);
                foreach (var uu in kvp.Value)
                {
                    yield return RAAUserRow(uu);
                }
            }
            foreach (var kvp in semiAssocCol)
            {
                foreach (var u in kvp.Key)
                {
                    yield return MlnUserRow(u);
                }
                foreach (var uu in kvp.Value)
                {
                    yield return RAAUserRow(uu);
                }
            }
        }

        private void SaveLNK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (step != 3)
                return;
            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream s = new CsvStream(assocsLines());
                using (var fs = File.Create(saveDlg.FileName))
                {
                    s.CopyTo(fs);
                }
            }
        }

        private void EchecLNK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (step != 3)
                return;
            if (saveDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Stream s = new CsvStream(echecsLines());
                using (var fs = File.Create(saveDlg.FileName))
                {
                    s.CopyTo(fs);
                }
            }
        }
    }
}
