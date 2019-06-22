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

        private void ReadFile(string fn)
        {
            switch (step)
            {
                case 0:
                    ReadRAA(fn);
                    break;
                case 1:
                    ReadESA(fn);
                    break;
                case 2:
                    ReadESTD(fn);
                    break;
                case 3:
                    ReadEPS(fn);
                    break;
                default:
                    break;
            }
        }

        private void ReadRAA(string fn)
        {

        }
        private void ReadESA(string fn)
        {

        }
        private void ReadESTD(string fn)
        {

        }
        private void ReadEPS(string fn)
        {

        }

    }
}
