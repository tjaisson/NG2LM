using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Publi4Par
{
    public partial class Fen : Form
    {
        int step = 0;

        public Fen()
        {
            InitializeComponent();
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
                Memo.AppendText(string.Format("Fichier lu, {0} personnes trouvées\r\n", i));
                i = RAAUsersCol.Count;
                if (i > 0)
                {
                    Memo.AppendText(string.Format("info : {0} cas de doublon Nom/Prénom détectés.\r\n", i));
                    /*i = 1;
                    foreach(var stck in RAAUsersCol.Values)
                    {
                        Memo.AppendText(string.Format("Cas {0} :\r\n", i++));
                        foreach (var p in stck)
                        {
                            Memo.AppendText(string.Format(" {0} {1}\r\n", p.Nom, p.Prenom));

                        }
                    }*/
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
                Memo.AppendText(string.Format("Fichier lu, {0} personnes trouvées\r\n", i));
                i = MLNUsersCol.Count;
                if (i > 0)
                {
                    Memo.AppendText(string.Format("info : {0} cas de doublon Nom/Prénom détectés.\r\n", i));
                    /*i = 1;
                    foreach(var stck in MLNUsersCol.Values)
                    {
                        Memo.AppendText(string.Format("Cas {0} :\r\n", i++));
                        foreach (var p in stck)
                        {
                            Memo.AppendText(string.Format(" {0} {1}\r\n", p.Nom, p.Prenom));

                        }
                    }*/
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
                Memo.AppendText("\r\nAnalyse...\r\n");
                foreach(var kvp in MLNUsers)
                {

                }


            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
