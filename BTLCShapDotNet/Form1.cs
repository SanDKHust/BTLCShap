using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLCShapDotNet
{
    public partial class Form1 : Form
    {
        private String fileText;
        public Form1()
        {
            InitializeComponent();
            textBoxSearch.ForeColor = Color.Silver;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bài Tập Lớn Môn Lập Trình .Net \nThành Viên Nhóm: \n 1.Dương Khắc San - 20156362" +
                "\n 2.Nguyễn Văn Cường - 2015...." +
                "\n Version: 1.0 :v");
        }

        private void textBoxSearch_Enter(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Equals("Search..."))
            {
                textBoxSearch.Text = "";
                textBoxSearch.ForeColor = Color.Black;
            }
        }

        private void textBoxSearch_Leave(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.Equals(""))
            {
                textBoxSearch.Text = "Search...";
                textBoxSearch.ForeColor = Color.Silver;
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stream readFile;
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if(openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if((readFile = openFileDialog.OpenFile()) != null)
                {
                    String fileName = openFileDialog.FileName;
                    fileText = File.ReadAllText(fileName);
                    richTextBox.Text = fileText;
                }
            }
        }
    }
}
