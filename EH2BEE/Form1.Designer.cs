namespace EH2BEE
{
    partial class Form1
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
            this.EchecLNK = new System.Windows.Forms.LinkLabel();
            this.SaveLNK = new System.Windows.Forms.LinkLabel();
            this.saveDlg = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // Memo
            // 
            this.Memo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Memo.Location = new System.Drawing.Point(12, 30);
            this.Memo.Multiline = true;
            this.Memo.Name = "Memo";
            this.Memo.Size = new System.Drawing.Size(776, 378);
            this.Memo.TabIndex = 0;
            // 
            // EchecLNK
            // 
            this.EchecLNK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.EchecLNK.AutoSize = true;
            this.EchecLNK.Location = new System.Drawing.Point(196, 428);
            this.EchecLNK.Name = "EchecLNK";
            this.EchecLNK.Size = new System.Drawing.Size(255, 13);
            this.EchecLNK.TabIndex = 5;
            this.EchecLNK.TabStop = true;
            this.EchecLNK.Text = "Générer un fichier avec les ambiguïtés et les échecs";
            this.EchecLNK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.EchecLNK_LinkClicked);
            // 
            // SaveLNK
            // 
            this.SaveLNK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.SaveLNK.AutoSize = true;
            this.SaveLNK.Location = new System.Drawing.Point(16, 428);
            this.SaveLNK.Name = "SaveLNK";
            this.SaveLNK.Size = new System.Drawing.Size(156, 13);
            this.SaveLNK.TabIndex = 4;
            this.SaveLNK.TabStop = true;
            this.SaveLNK.Text = "Générer le fichier ELEGROUPE";
            this.SaveLNK.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SaveLNK_LinkClicked);
            // 
            // saveDlg
            // 
            this.saveDlg.DefaultExt = "csv";
            this.saveDlg.Filter = "Fichiers CSV|*.csv";
            // 
            // Form1
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EchecLNK);
            this.Controls.Add(this.SaveLNK);
            this.Controls.Add(this.Memo);
            this.Name = "Form1";
            this.Text = "Form1";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox Memo;
        private System.Windows.Forms.LinkLabel EchecLNK;
        private System.Windows.Forms.LinkLabel SaveLNK;
        private System.Windows.Forms.SaveFileDialog saveDlg;
    }
}

