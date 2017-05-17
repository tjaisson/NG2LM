namespace Lilie2LaboMep
{
    partial class Fen
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            this.openFileDlg = new System.Windows.Forms.OpenFileDialog();
            this.Classe_CB = new System.Windows.Forms.CheckedListBox();
            this.saveFileDlg = new System.Windows.Forms.SaveFileDialog();
            this.RNE_TB = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AllLnk = new System.Windows.Forms.LinkLabel();
            this.NONELnk = new System.Windows.Forms.LinkLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // Classe_CB
            // 
            this.Classe_CB.CheckOnClick = true;
            this.Classe_CB.FormattingEnabled = true;
            this.Classe_CB.Location = new System.Drawing.Point(31, 62);
            this.Classe_CB.MultiColumn = true;
            this.Classe_CB.Name = "Classe_CB";
            this.Classe_CB.Size = new System.Drawing.Size(549, 274);
            this.Classe_CB.TabIndex = 2;
            // 
            // saveFileDlg
            // 
            this.saveFileDlg.DefaultExt = "csv";
            this.saveFileDlg.Filter = "Fichiers CSV|*.csv";
            // 
            // RNE_TB
            // 
            this.RNE_TB.Location = new System.Drawing.Point(349, 351);
            this.RNE_TB.Name = "RNE_TB";
            this.RNE_TB.Size = new System.Drawing.Size(100, 20);
            this.RNE_TB.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 352);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(334, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "3. Saisir le RNE de l\'établissement (ex. 0750677D) :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(252, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "2. Sélectionner les classes à exporter :";
            // 
            // AllLnk
            // 
            this.AllLnk.AutoSize = true;
            this.AllLnk.Location = new System.Drawing.Point(28, 46);
            this.AllLnk.Name = "AllLnk";
            this.AllLnk.Size = new System.Drawing.Size(40, 13);
            this.AllLnk.TabIndex = 7;
            this.AllLnk.TabStop = true;
            this.AllLnk.Text = "Toutes";
            this.AllLnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.AllLnk_LinkClicked);
            // 
            // NONELnk
            // 
            this.NONELnk.AutoSize = true;
            this.NONELnk.Location = new System.Drawing.Point(74, 46);
            this.NONELnk.Name = "NONELnk";
            this.NONELnk.Size = new System.Drawing.Size(44, 13);
            this.NONELnk.TabIndex = 8;
            this.NONELnk.TabStop = true;
            this.NONELnk.Text = "Aucune";
            this.NONELnk.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.NONELnk_LinkClicked);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.LinkArea = new System.Windows.Forms.LinkArea(3, 6);
            this.linkLabel1.Location = new System.Drawing.Point(12, 9);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(549, 20);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "1. Ouvrir le fichier \"export générique\" des élèves de Lilie ou le déposer dans ce" +
    "tte fenêtre.";
            this.linkLabel1.UseCompatibleTextRendering = true;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // linkLabel2
            // 
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel2.LinkArea = new System.Windows.Forms.LinkArea(3, 7);
            this.linkLabel2.Location = new System.Drawing.Point(12, 374);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(360, 20);
            this.linkLabel2.TabIndex = 12;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "4. Générer le fichier destiné à être importé dans Labomep.";
            this.linkLabel2.UseCompatibleTextRendering = true;
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Location = new System.Drawing.Point(532, 46);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(48, 13);
            this.linkLabel3.TabIndex = 13;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "à propos";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            // 
            // Fen
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(592, 400);
            this.Controls.Add(this.linkLabel3);
            this.Controls.Add(this.linkLabel2);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.NONELnk);
            this.Controls.Add(this.AllLnk);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RNE_TB);
            this.Controls.Add(this.Classe_CB);
            this.Name = "Fen";
            this.Text = "Convertisseur Lilie -> Labomep";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Fen_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Fen_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDlg;
        private System.Windows.Forms.CheckedListBox Classe_CB;
        private System.Windows.Forms.SaveFileDialog saveFileDlg;
        private System.Windows.Forms.TextBox RNE_TB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.LinkLabel AllLnk;
        private System.Windows.Forms.LinkLabel NONELnk;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.LinkLabel linkLabel3;
    }
}

