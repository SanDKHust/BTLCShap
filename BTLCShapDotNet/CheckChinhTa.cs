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
        public static string[] VietCharacters = { "a", "à", "á", "ả", "ã", "ạ", "ă", "ắ", "ặ", "ẳ", "ằ", "ẵ", "â", "ầ", "ấ", "ẩ", "ẫ", "ậ", "b",
            "c", "d", "đ", "e", "è", "é", "ẻ", "ẽ", "ẹ", "ê", "ề", "ế", "ể", "ệ", "ễ", "g", "h", "i", "ì", "í", "ĩ", "ỉ", "ị", "k", "l", "m",
            "n", "o", "ò", "ó", "ỏ", "õ", "ọ", "ô", "ồ", "ố", "ổ", "ỗ", "ộ", "ơ", "ờ", "ớ", "ở", "ỡ", "ợ", "p", "q", "r", "s", "t", "u", "ù",
            "ú", "ủ", "ũ", "ụ", "ư", "ừ", "ứ", "ữ", "ử", "ự", "v", "x", "y", "ỳ", "ý", "ỷ", "ỹ", "ỵ" };

        private string[] am_dau = new string[]
            {
                "ngh", "ch", "gh", "kh", "ng", "nh", "ph", "th", "tr",
                "b", "c", "d", "đ", "g", "h", "k", "l", "m", "n", "p",
                "q", "r", "s", "t", "v", "x", "none"
            };

        private string[] am_giua = new string[]
            {
                "a", "ai", "ao", "ay", "au",
                "e", "eo",
                "i", "iu", "ia", "io", "iêu", "iê",
                "o", "oă", "oe", "oi", "oai", "oay", "oa", "oo", "oeo",
                "u", "ua", "ue", "ui", "ươ", "uy", "uôi", "uyê", "uay", "uyu", "uya", "uai",
                "ă", "â", "ê", "ây", "âu", "êu", "ôi", "oay",
                "y", "ô", "ơ", "u", "ư", "âu", "ây", "êu", "ai",
                "ôi", "ơi", "ua","uô", "uâ","ưa","ươ","uê","ui",
                "ưi", "ưu", "uy", "yêu", "uây","uôi", "ươi", "ươu","uyê"
            };

        public string[] am_cuoi = new string[] { "ch", "ng", "nh", "c", "m", "n", "p", "t", "none" };

        private static string[] phaiThemAmCuoi = new string[] { "â", "ă", "iê", "uâ", "ươ", "yê", "oă", "oo", "uă", "uyê" };

        public static string[] khongCoAmCuoi = new string[]
            {
                "ai", "ao", "au", "âu", "ay", "ây", "eo", "êu", "ia", "iu", "oi", "ôi", "ơi", "ưa", "ui", "ưi", "ưu",
                "iêu", "yêu", "oai", "oao", "oay", "oeo", "uai", "uây", "uôi", "ươi", "ươu", "uya", "uyu", "uay", "oay"
            };

        private string[] errorAmDauGiua = new string[] {
            "ghiêu", "ciêu", "giêu", "qiêu", "ghoai", "coai", "goai", "koai", "poai", "choay", "ngoay", "boay", "coay", "moay", "qoay", "poay", "roay",
            "toay", "voay", "ghâu", "qâu", "kâu", "ghưu","dưu", "gưu","pưu", "qưu", "vưu", "chươi", "ghươi", "trươi", "xươi", "sươi", "pươi", "qươi", "kươi"
        };
        private string[] errorAmGiuaCuoi = new string[] {
            "oăch", "oănh", "ioch", "ionh", "iêch", "iênh", "oech", "oeng", "oenh", "uyêch", "uyêng", "uyênh", "uyêm", "uyêp"
        };

        public string AmDau = "";
        public string AmGiua = "";
        public string AmCuoi = "";

        public int SearchString(string key)
        {
            Array.Sort(am_giua);
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

        public static string tachDauThanh(string text)
        {
            string[] arr1 = new string[] { "á", "à", "ả", "ã", "ạ", "ấ", "ầ", "ẩ", "ẫ", "ậ", "ắ", "ằ", "ẳ", "ẵ", "ặ",
                "é","è","ẻ","ẽ","ẹ","ế","ề","ể","ễ","ệ", "í","ì","ỉ","ĩ","ị","ó","ò","ỏ","õ","ọ","ố","ồ","ổ","ỗ","ộ",
                "ớ","ờ","ở","ỡ","ợ", "ú","ù","ủ","ũ","ụ","ứ","ừ","ử","ữ","ự", "ý","ỳ","ỷ","ỹ","ỵ"};

            string[] arr2 = new string[] { "a", "a", "a", "a", "a", "â", "â", "â", "â", "â", "ă", "ă", "ă", "ă", "ă",
                "e","e","e","e","e","ê","ê","ê","ê","ê", "i","i","i","i","i", "o","o","o","o","o","ô","ô","ô","ô","ô",
                "ơ","ơ","ơ","ơ","ơ", "u","u","u","u","u","ư","ư","ư","ư", "ư", "y","y","y","y","y"};
            for (int i = 0; i < arr1.Length; i++)
            {
                text = text.Replace(arr1[i], arr2[i]);
            }
            return text;
        }

        public int tachAm(string inputWord)
        {
            int isWord = -1;
            int res = -1;
            string word = tachDauThanh(inputWord);

            //Trường hợp [none] - [Âm giữa] - [none]
            if (word.Length <= 3)
            {
                res = SearchString(word);
                if (res > 0) AmGiua = am_giua[res];
            }
            if (res < 0)
            {
                string[] am_tiet = word.ToCharArray().Select(c => c.ToString()).ToArray();
                for (int j = 0; j < am_tiet.Length; j++)
                {
                    am_tiet[j] = am_tiet[j].ToLower();
                }
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
                if (AmCuoi == "" && AmDau != "")
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
                if (AmDau == "" && AmCuoi != "")
                {
                    var tmp_am_giua = word.Replace(AmCuoi, "");
                    isWord = SearchString(tmp_am_giua);
                    if (isWord > -1) { AmGiua = tmp_am_giua; }
                }
                else
                //Trường hợp [none] - [Âm giữa] - [none]
                if (AmDau == "" && AmCuoi == "")
                {
                    isWord = SearchString(word);
                    if (isWord > -1) { AmGiua = word; }
                }
                else
                {
                    //Trường hợp [has] - [Âm giữa] - [has]
                    var tmp_am_giua = word.ToLower().Replace(AmDau, "").Replace(AmCuoi, "");
                    isWord = SearchString(tmp_am_giua);
                    if (isWord > -1) { AmGiua = tmp_am_giua; }
                }
            }
            return isWord;
        }

        public bool kiemTraChinhTa(string word)
        {
            // Check co nam trong bang chu cai hay khong
            if (word.Length > 7)
            {
                return false;
            }
            for (int i = 0; i < word.Length; i++)
            {
                if (!VietCharacters.Contains(word[i] + ""))
                {
                    return false;
                }
            }
            int isWord = tachAm(word);
            // Check ko the tach am hoac khong co am giua
            if (isWord == -1 || AmGiua == "")
            {
                return false;
            }
            int seperateLenght = this.AmGiua.Length + this.AmDau.Length + this.AmCuoi.Length;
            // Check lai ket qua tach am
            if (seperateLenght != word.Length)
            {
                return false;
            }
            if (AmGiua.Equals("yêu"))
            {
                if (AmDau != "" || AmCuoi != "")
                {
                    return false;
                }
            }
            // Check am giua co hay khong the co am cuoi
            if (phaiThemAmCuoi.Contains(AmGiua))
            {
                if (AmCuoi == "")
                {
                    return false;
                }
            }
            if (khongCoAmCuoi.Contains(AmGiua))
            {
                if (AmGiua != "")
                {
                    return false;
                }
            }
            foreach (String adg in errorAmDauGiua)
            {
                if (this.AmDau + this.AmGiua == adg)
                {
                    return false;
                }
            }
            foreach (String agc in errorAmGiuaCuoi)
            {
                if (this.AmGiua + this.AmCuoi == agc)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
