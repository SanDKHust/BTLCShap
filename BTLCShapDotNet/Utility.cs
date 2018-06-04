using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLCShapDotNet
{
    public static class Utility
    {
        public static void searchHighlightText(this RichTextBox myRtb, string word, Color color, bool isSearch)
        {

            if (word == string.Empty)
                return;

            string textSearch = myRtb.Text.ToString().ToLower();
            string search = word.ToLower();

            if (textSearch.Contains(search))
            {
                int s_start = myRtb.SelectionStart, startIndex = 0, index;
                if (isSearch)
                {
                    while ((index = textSearch.IndexOf(search, startIndex)) != -1)
                    {
                        myRtb.Select(index, search.Length);
                        myRtb.SelectionBackColor = color;
                        if (startIndex == 0) myRtb.ScrollToCaret();
                        startIndex = index + search.Length;
                    }
                }
                else
                {

                    Regex rx = new Regex(" "+search+" ");
                    foreach (Match match in rx.Matches(textSearch))
                    {
                        index = match.Index;
                        myRtb.Select(index+1, search.Length);
                        myRtb.SelectionColor = color;
                        myRtb.SelectionFont = new Font("Italic", 10, FontStyle.Underline);
                        startIndex = index + search.Length;
                    }

                    //while ((index = textSearch.IndexOf(search, startIndex)) != -1)
                    //{
                    //    myRtb.Select(index, search.Length);
                    //    myRtb.SelectionColor = color;
                    //    myRtb.SelectionFont = new Font("Italic", 10, FontStyle.Underline);
                    //    startIndex = index + search.Length;
                    //}
                }

            }

            //myRtb.SelectionStart = s_start;
            //myRtb.SelectionLength = 0;
            //myRtb.SelectionColor = color;

        }

        public static DataSet searchListHoiByNoiDungHoi(string text, int idTruyen)
        {
            DataSet dataSet = new DataSet();

            string query = "select id,TenHoi from Hoi where idTruyen = " + idTruyen + " and NoiDung like N'% " + text + " %'";
            using (SqlConnection connection = new SqlConnection(ConnectionString.connectionString))
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(query, connection);
                adapter.Fill(dataSet, "listHoi");
                connection.Close();
            }
            return dataSet;
        }
    }
}
