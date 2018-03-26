﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BTLCShapDotNet
{
    public static class Utility
    {
        public static void searchHighlightText(this RichTextBox myRtb, string word, Color color,bool isSearch)
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
                        startIndex = index + search.Length;
                    }
                }
                else
                {
                    while ((index = textSearch.IndexOf(search, startIndex)) != -1)
                    {
                        myRtb.Select(index, search.Length);
                        myRtb.SelectionColor = color;
                        myRtb.SelectionFont = new Font("Italic", 10, FontStyle.Underline);
                        startIndex = index + search.Length;
                    }
                }

            }
            
            //myRtb.SelectionStart = s_start;
            //myRtb.SelectionLength = 0;
            //myRtb.SelectionColor = color;
            
        }
    }
}