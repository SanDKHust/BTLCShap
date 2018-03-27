using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTLCShapDotNet
{
    class CheckChinhTa
    {
        private string[] am_dau = new string[]{"ngh",
                                      "ch", "gh", "gi", "kh", "ng", "nh", "ph", "qu", "th", "tr",
                                      "b", "c", "d", "đ", "g", "h", "k", "l", "m", "n", "p", "q", "r", "s", "t", "v", "x",
                                      "none"
                                    };
        public string[] am_cuoi = new string[] { "ch", "ng", "nh", "c", "m", "n", "p", "t", "none" };
        //public string[] am_giua = new string[] { "a", "á", "à", "â", "ai", "ao", "au", "âu", "ay", "ây", "e", "ê", "eo", "êu", "i", "iai", "iu", "o", "ô", "o", "oa", "oa", "oai", "oay", "oe", "oi", "ôi", "oi", "oo", "ôô", "u", "u", "ua", "uâ", "ua", "uây", "uê", "ui", "ui", "uô", "uo", "uôi", "ười", "uou", "uu", "uy", "uyê", "uyề", "y", "yê", "yêu" };

        private string[] am_giua = new string[]{ "a", "ă", "â", "e", "ê", "i", "y", "o", "ô", "ơ", "u", "ư",
                                        "ai", "ao", "au", "âu", "ay", "ây", "eo", "êu", "ai","yê", "iu", "oa", "oă", "oe", "oi", "ôi", "ơi", "oo", "ôô", "ua","uô", "uâ","ưa","ươ","uê","ui","ưi", "ưu", "uy",
                                        "yêu", "oai", "oay","uây","uôi", "ươi", "ươu","uyê"};

        public string AmDau = "";
        public string AmGiua = "";
        public string AmCuoi = "";


        public int SearchString(string key)
        {
            int first = 0;
            int last = am_giua.Length;

            while (first < last)
            {
                int mid = (first + last) / 2;
                if (key.CompareTo(am_giua[mid]) < 0)
                {
                    last = mid;
                }
                else if (key.CompareTo(am_giua[mid]) > 0)
                {
                    first = mid + 1;
                }
                else
                {
                    return mid;
                }
            }
            return -(first + 1);
        }

        public int tachAm(string word)
        {
            int isWord = -1;

            //Trường hợp [none] - [Âm giữa] - [none]
            if (word.Length <= 3)
            {
                int res = SearchString(word);
                if (res > 0) AmGiua = am_giua[res];
            }
            else if (isWord < 0)
            {
                string[] am_tiet = word.ToCharArray().Select(c => c.ToString()).ToArray();
                //tìm âm đầu
                foreach (String ad in am_dau)
                {
                    if (am_tiet.Length > 3 && ad.Length == 3)
                    {
                        if (am_tiet[0] + am_tiet[1] + am_tiet[2] == ad)
                        {
                            AmDau = ad;
                            break;
                        }
                    }

                    if (am_tiet.Length > 2 && ad.Length == 2)
                    {
                        if (am_tiet[0] + am_tiet[1] == ad)
                        {
                            AmDau = ad;
                            break;
                        }

                    }

                    if (am_tiet[0] == ad && ad.Length == 1)
                    {
                        AmDau = ad;
                        break;
                    }
                }

                //tìm âm cuối
                foreach (String ac in am_cuoi)
                {
                    if (am_tiet.Length > 2 && ac.Length == 2)
                    {
                        if (am_tiet[am_tiet.Length - 2] + am_tiet[am_tiet.Length - 1] == ac)
                        {
                            AmCuoi = ac;
                            break;
                        }
                    }
                    if (am_tiet[am_tiet.Length - 1] == ac && ac.Length == 1)
                    {
                        AmCuoi = ac;
                        break;
                    }

                }
                //Trường hợp [has] - [Âm giữa] - [none]
                if (AmCuoi == "")
                {

                    var tmp_am_giua = word.Replace(AmDau, "");
                    isWord = SearchString(tmp_am_giua);
                    if (isWord > -1)
                    {
                        AmGiua = tmp_am_giua;
                    }

                }
                else

                //Trường hợp [none] - [Âm giữa] - [has]

                if (AmDau == "")
                {
                    var tmp_am_giua = word.Replace(AmCuoi, "");
                    isWord = SearchString(tmp_am_giua);
                    if (isWord > -1) { AmGiua = tmp_am_giua; }
                }
                else
                {
                    //Trường hợp [has] - [Âm giữa] - [has]
                    var tmp_am_giua = word.Replace(AmDau, "").Replace(AmCuoi, "");
                    isWord = SearchString(tmp_am_giua);
                    if (isWord > -1) { AmGiua = tmp_am_giua; }
                }
            }
            return isWord;
        }
    }
}
