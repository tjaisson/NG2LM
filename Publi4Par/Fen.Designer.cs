namespace Publi4Par
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
            this.Memo = new System.Windows.Forms.TextBox();
            this.SaveLNK = new System.Windows.Forms.LinkLabel();
            this.saveDlg = new System.Windows.Forms.SaveFileDialog();
            this.EchecLNK = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // Memo
            // 
            this.Memo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Memo.Location = new System.Drawing.Point(12, 12);
            this.Memo.Multiline = true;
            this.Memo.Name = "Memo";
            this.Memo.ReadOnly = true;
            this.Memo.Size = new System.Drawing.Size(477, 337);
            this.Memo.TabIndex = 0;
            // 
            // SaveLNK
            // 
            this.SaveLNK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveLNK.AutoSize = true;
            this.SaveLNK.Location = new System.Drawing.Point(12, 352);
            this.SaveLNK.Name = "SaveLNK";
            this.SaveLNK.Size = new System.Drawing.Size(111, 13);
            this.SaveLNK.TabIndex = 2;
            this.SaveLNK.TabStop = true;
            this.SaveLNK.Text = "Générer le fichier CSV";
            this.SaveLNK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SaveLNK_LinkClicked);
            // 
            // saveDlg
            // 
            this.saveDlg.DefaultExt = "csv";
            this.saveDlg.Filter = "Fichiers CSV|*.csv";
            // 
            // EchecLNK
            // 
            this.EchecLNK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EchecLNK.AutoSize = true;
            this.EchecLNK.Location = new System.Drawing.Point(192, 352);
            this.EchecLNK.Name = "EchecLNK";
            this.EchecLNK.Size = new System.Drawing.Size(255, 13);
            this.EchecLNK.TabIndex = 3;
            this.EchecLNK.TabStop = true;
            this.EchecLNK.Text = "Générer un fichier avec les ambiguïtés et les échecs";
            this.EchecLNK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EchecLNK_LinkClicked);
            // 
            // Fen
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 374);
            this.Controls.Add(this.EchecLNK);
            this.Controls.Add(this.SaveLNK);
            this.Controls.Add(this.Memo);
            this.Name = "Fen";
            this.Text = "Publi4Par";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Fen_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Fen_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Memo;
        private System.Windows.Forms.LinkLabel SaveLNK;
        private System.Windows.Forms.SaveFileDialog saveDlg;
        private System.Windows.Forms.LinkLabel EchecLNK;
    }
}

