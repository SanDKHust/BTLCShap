﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLCShapDotNet
{
    public partial class Form1 : Form
    {
        private String richTextBoxString;
        private string strSearch = string.Empty;
       
        List<string> listTenTruyen = null;
        List<int> listIdTruyen = null;
        List<string> listHoi = null;
        List<int> listIdHoi = null;


        public Form1()
        {
            InitializeComponent();
            
            listIdTruyen = getListTruyen().Tables["listTruyen"].AsEnumerable().Select(r => r.Field<int>("id")).ToList();
            listTenTruyen = getListTruyen().Tables["listTruyen"].AsEnumerable().Select(r => r.Field<string>("name")).ToList();
            comboBoxTryen.DataSource = listTenTruyen;

            listViewHoi.View = View.List;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Bài Tập Lớn Môn Lập Trình .Net \nThành Viên Nhóm: \n 1.Dương Khắc San - 20156362" +
                "\n 2.Nguyễn Văn Cường - 2015...." +
                "\nVersion: 1.0 :v");
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
                    richTextBoxString = File.ReadAllText(fileName);
                    richTextBoxString = Regex.Replace(richTextBoxString, "<p>", " ");
                    richTextBoxString = Regex.Replace(richTextBoxString, "</p>", "\n");
                    richTextBox.Text = richTextBoxString;
                }
            }
        }

        public void checkDoanVan(String str)
        {
            var s = str.ToLower();
            s = Regex.Replace(s, @"[^\w\d]", " ");
            s = Regex.Replace(s, @"[\d-]", string.Empty);
            while (s.IndexOf("  ") >= 0)    //tim trong chuoi vi tri co 2 khoang trong tro len      
                s = s.Replace("  ", " ");   //sau do thay the bang 1 khoang trong          
            s = s.Trim();
            //richTextBox.Text = s;

            List<string> arrWord = s.Split(' ').ToList();
            List<string> listWordCheck = arrWord.Distinct().ToList();

            foreach (string word in listWordCheck)
            {
                if (!checkWord(word)) Utility.searchHighlightText(richTextBox, word, Color.Red, false);
            }

            //arr1.Sort((s1, s2) => s1.CompareTo(s2));

        }

        private bool checkWord(String word)
        {
            return true;
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            strSearch = textBoxSearch.Text.ToString().Trim();
            Utility.searchHighlightText(richTextBox, strSearch, Color.Yellow,true);
        }

        private void textBoxSearch_TextChanged(object sender, EventArgs e)
        {
            if (textBoxSearch.Text.ToString() == string.Empty)
            {
                if(strSearch != string.Empty)
                {
                    richTextBox.Text = richTextBoxString;
                    strSearch = string.Empty;
                }
            }
        }


        private void textBoxSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                strSearch = textBoxSearch.Text.ToString().Trim();
                Utility.searchHighlightText(richTextBox, strSearch, Color.Yellow,true);
            }
          
        }

        private DataSet getListTruyen()
        {
            DataSet dataSet = new DataSet();
            
            string query = "select * from Truyen";
            using(SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dataSet,"listTruyen");
                connection.Close();
            }
            return dataSet;
        }

        private void insert()
        {

            System.Data.SqlClient.SqlConnection sqlConnection1 = new System.Data.SqlClient.SqlConnection(ConnectionString.connectionString);

            System.Data.SqlClient.SqlCommand cmd = new System.Data.SqlClient.SqlCommand();
            cmd.CommandType = System.Data.CommandType.Text;
            cmd.CommandText = "INSERT Hoi (id,idTruyen,TenHoi,NoiDung) VALUES(5,1," + "N'Hồi 05: Cung loan bắn điêu',N'" + richTextBoxString + "')";
            cmd.Connection = sqlConnection1;

            sqlConnection1.Open();
            cmd.ExecuteNonQuery();
            sqlConnection1.Close();
            
        }

        private void comboBoxTryen_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox cb = sender as ComboBox;
            if(cb.SelectedValue != null)
            {
                String s = cb.SelectedValue as String;
                for(int i = 0; i < listTenTruyen.Count; i++)
                {
                    if (listTenTruyen[i].Equals(s))
                    {
                        listViewHoi.Clear();
                        if(listIdTruyen != null)
                        {
                            listHoi = getListHoi(listIdTruyen[i]).Tables["listHoi"].AsEnumerable().Select(r => r.Field<string>("TenHoi")).ToList();
                            listIdHoi = getListHoi(listIdTruyen[i]).Tables["listHoi"].AsEnumerable().Select(r => r.Field<int>("id")).ToList();
                            foreach (string str in listHoi) listViewHoi.Items.Add(str);
                        }
                        break;
                    }
                }
            }
        }

        private DataSet getListHoi(int idTruyen)
        {
            DataSet dataSet = new DataSet();

            string query = "select id,TenHoi from Hoi where idTruyen = "+idTruyen;
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dataSet, "listHoi");
                connection.Close();
            }
            return dataSet;
        }

        private string getNoiDungHoi(int idHoi)
        {
            DataTable dataTable = new DataTable();

            string query = "select NoiDung from Hoi where id = " + idHoi;
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dataTable);
                connection.Close();
            }
            return dataTable.Rows[0]["NoiDung"].ToString();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            insert();
        }

        private void listViewHoi_Click(object sender, EventArgs e)
        {
            if (listViewHoi.SelectedItems.Count > 0)
            {
                for (int i = 0; i < listHoi.Count; i++)
                {
                    if (listHoi[i].Equals(listViewHoi.SelectedItems[0].Text.ToString()))
                    {
                        richTextBox.Text = getNoiDungHoi(listIdHoi[i]);
                        richTextBoxString = richTextBox.Text.ToString();
                    }
                }
            }
        }
    }
}
