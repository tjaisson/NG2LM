using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Lilie2LaboMep
{
    public partial class Fen : Form
    {
        public Fen()
        {
            InitializeComponent();
        }

        private void Fen_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
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

        private void DoOpen()
        {
            if (openFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                ReadFile(openFileDlg.FileName);
            }
        }

        private void ReadFile(string fileName)
        {
            Classe_CB.Items.Clear();
            CsvReader RD = new CsvReader(fileName);
            if (!RD.read())
                MessageBox.Show("Le fichier ne semble pas avoir le bon format");
            foreach (CsvClasse Classe in RD.Liste_Classes.Values)
            {
                Classe_CB.Items.Add(Classe, true);
            }
        }

        private void DoSave()
        {
            if (Classe_CB.CheckedItems.Count <= 0)
            {
                MessageBox.Show("Aucune classe n'est sélectionnée.");
                return;
            }
            RNE_TB.Text = RNE_TB.Text.Trim();
            if (RNE_TB.Text == "")
            {
                MessageBox.Show("Veuillez saisir le RNE de l'établissement.");
                return;
            }
            if (saveFileDlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var utf8WithoutBom = new System.Text.UTF8Encoding(false);
                using (StreamWriter sw = new StreamWriter(File.Open(saveFileDlg.FileName, FileMode.Create), utf8WithoutBom))
                //using (StreamWriter sw = new StreamWriter(File.Open(saveFileDlg.FileName, FileMode.Create), System.Text.Encoding.UTF8))
                {
                    sw.NewLine = "\n";
                    foreach (CsvClasse Classe in Classe_CB.CheckedItems)
                    {
                        foreach (CsvUser u in Classe.Eleves)
                            sw.WriteLine(u.Id + ";" + u.Nom + ";" + u.Prenom + ";" + u.Nom_Classe + ";" + RNE_TB.Text);
                    }
                }
                MessageBox.Show("Le fichier a bien été généré.");
            }
        }

        private void AllLnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            for (int i = 0; i < Classe_CB.Items.Count; i++)
            {
                Classe_CB.SetItemCheckState(i, CheckState.Checked);
            }
        }

        private void NONELnk_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            foreach (int i in Classe_CB.CheckedIndices)
            {
                Classe_CB.SetItemCheckState(i, CheckState.Unchecked);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DoOpen();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            DoSave();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            using(AboutBox abBox = new AboutBox())
            {
                abBox.ShowDialog();
            }
        }
    }
}
