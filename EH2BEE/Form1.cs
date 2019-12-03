using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using Classes;

namespace EH2BEE
{
    public partial class Form1 : Form
    {
        int step = 0;

        public Form1()
        {
            InitializeComponent();
            SaveLNK.Hide();
            EchecLNK.Hide();
            Memo.AppendText("Pour commencer, veuillez déposer ici le fichier Export Educ Horus (csv)\r\n");
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if ((step == 0 || step == 1) && (e.Data.GetDataPresent(DataFormats.FileDrop)))
                e.Effect = DragDropEffects.Copy;
            else
                e.Effect = DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
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
                    ReadEH(fn);
                    break;
                case 1:
                    ReadBEE(fn);
                    break;
                default:
                    break;
            }
        }

        private Dictionary<NPD, EducCsvReader.TEducUser> EHUsers;
        private Dictionary<NPD, Stack<EducCsvReader.TEducUser>> EHUsersCol;

        private void ReadEH(string fn)
        {
            EHUsers = new Dictionary<NPD, EducCsvReader.TEducUser>();
            EHUsersCol = new Dictionary<NPD, Stack<EducCsvReader.TEducUser>>();
            try
            {
                int i = 0;
                Memo.AppendText("Lecture fichier...\r\n");
                foreach (var u in (new EducCsvReader(fn)))
                {
                    i++;
                    NPD npd = new NPD(stringManip.simplifyName(u.nom), stringManip.simplifyName(u.prenom), u.DateNaiss);
                    Stack<EducCsvReader.TEducUser> stck;
                    if (EHUsersCol.TryGetValue(npd, out stck))
                    {
                        stck.Push(u);
                    }
                    else if (EHUsers.TryGetValue(npd, out var uu))
                    {
                        EHUsers.Remove(npd);
                        stck = new Stack<EducCsvReader.TEducUser>();
                        stck.Push(uu);
                        stck.Push(u);
                        EHUsersCol.Add(npd, stck);
                    }
                    else
                    {
                        EHUsers.Add(npd, u);
                    }
                }
                if (i == 0)
                {
                    Memo.AppendText(string.Format("Fichier Vide...\r\n", i));
                    return;
                }
                Memo.AppendText(string.Format("Fichier lu, {0} personnes trouvées\r\n", i));
                i = EHUsersCol.Count;
                if (i > 0)
                {
                    Memo.AppendText(string.Format("info : {0} cas d'homonymes Nom & Prénom & Date détectés.\r\n", i));
                }
                Memo.AppendText("\r\nPour continuer,  déposer ici le fichier ExportXML « ElevesSansAdresse » (zip ou xml)\r\n");
                step = 1;

            }
            catch (Exception)
            {
                Memo.AppendText("Le fichier n'a pas le bon format\r\n");
            }
        }

        private Dictionary<NPD, TBEEFile.TBEEUser> BEEUsers;
        private Dictionary<NPD, Stack<TBEEFile.TBEEUser>> BEEUsersCol;
        private Dictionary<EducCsvReader.TEducUser, TBEEFile.TBEEUser> assoc;
        private Dictionary<EducCsvReader.TEducUser, Stack<TBEEFile.TBEEUser>> semiAssoc;
        private Stack<EducCsvReader.TEducUser> echec;
        private Dictionary<Stack<EducCsvReader.TEducUser>, TBEEFile.TBEEUser> assocCol;
        private Dictionary<Stack<EducCsvReader.TEducUser>, Stack<TBEEFile.TBEEUser>> semiAssocCol;
        private Stack<Stack<EducCsvReader.TEducUser>> echecCol;

        private void ReadBEE(string fn)
        {
            BEEUsers = new Dictionary<NPD, TBEEFile.TBEEUser>();
            BEEUsersCol = new Dictionary<NPD, Stack<TBEEFile.TBEEUser>>();
            try
            {
                int i = 0;
                Memo.AppendText("Lecture fichier...\r\n");
                foreach (var u in (new TBEEFile(fn)))
                {
                    i++;
                    NPD npd = new NPD(stringManip.simplifyName(u.nom), stringManip.simplifyName(u.prenom), u.DateNaiss);
                    Stack<TBEEFile.TBEEUser> stck;
                    if (BEEUsersCol.TryGetValue(npd, out stck))
                    {
                        stck.Push(u);
                    }
                    else if (BEEUsers.TryGetValue(npd, out var uu))
                    {
                        BEEUsers.Remove(npd);
                        stck = new Stack<TBEEFile.TBEEUser>();
                        stck.Push(uu);
                        stck.Push(u);
                        BEEUsersCol.Add(npd, stck);
                    }
                    else
                    {
                        BEEUsers.Add(npd, u);
                    }
                }
                if (i == 0)
                {
                    Memo.AppendText(string.Format("Fichier Vide...\r\n", i));
                    return;
                }
                Memo.AppendText(string.Format("Fichier lu, {0} personnes trouvées\r\n", i));
                i = EHUsersCol.Count;
                if (i > 0)
                {
                    Memo.AppendText(string.Format("info : {0} cas d'homonymes Nom & Prénom & Date détectés.\r\n", i));
                }
                Memo.AppendText("\r\nPour continuer,  déposer ici le fichier ExportXML « ElevesSansAdresse » (zip ou xml)\r\n");
                step = 1;

            }
            catch (Exception)
            {
                Memo.AppendText("Le fichier n'a pas le bon format\r\n");
            }

            try
            {
                Memo.AppendText("\r\nMise en correspondance...\r\n");
                int a = 0;
                int sa = 0;
                int e = 0;

                assoc = new Dictionary<EducCsvReader.TEducUser, TBEEFile.TBEEUser>();
                semiAssoc = new Dictionary<EducCsvReader.TEducUser, Stack<TBEEFile.TBEEUser>>();
                echec = new Stack<EducCsvReader.TEducUser>();
                assocCol = new Dictionary<Stack<EducCsvReader.TEducUser>, TBEEFile.TBEEUser>();
                semiAssocCol = new Dictionary<Stack<EducCsvReader.TEducUser>, Stack<TBEEFile.TBEEUser>>();
                echecCol = new Stack<Stack<EducCsvReader.TEducUser>>();

                foreach (var kvp in EHUsers)
                {
                    if (BEEUsers.TryGetValue(kvp.Key, out var u))
                    {
                        assoc.Add(kvp.Value, u);
                        a++;
                    }
                    else if (BEEUsersCol.TryGetValue(kvp.Key, out var uc))
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
                foreach (var kvp in EHUsersCol)
                {
                    int nb = kvp.Value.Count;
                    if (BEEUsers.TryGetValue(kvp.Key, out var u))
                    {
                        assocCol.Add(kvp.Value, u);
                        sa += nb;
                    }
                    else if (BEEUsersCol.TryGetValue(kvp.Key, out var uc))
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
                BEEUsers = null;
                BEEUsersCol = null;
                EHUsers = null;
                EHUsersCol = null;
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

        private void SaveLNK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (step != 3)
                return;
            saveDlg.Filter = "Fichiers XML|*.xml";
            if (saveDlg.ShowDialog() == DialogResult.OK)
            {
                TEleGrFile EleGrFile = new TEleGrFile();
                EleGrFile.Init();

                foreach(var kvp in assoc)
                {
                    if (!string.IsNullOrEmpty(kvp.Value.division))
                        if (kvp.Key.Groups.Count > 0)
                            EleGrFile.AddEleve(kvp.Key, kvp.Value);
                }
                EleGrFile.SaveToFile(saveDlg.FileName);
            }
        }

        private void EchecLNK_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
