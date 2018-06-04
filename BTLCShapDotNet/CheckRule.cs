using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BTLCShapDotNet
{
    class CheckRule
    {
        public static string[] Check(string lines)
        {
            string[] words = GetWords(lines);
            List<String> wrongWords = new List<string>();
            int len = words.Length;
            Rule rule;

            for (int i = 0; i < len; i++)
            {
                rule = new Rule();
                string temp = words[i].ToLower();

                while (rule.index < temp.Length)
                {
                    rule = rule.Check(temp);
                }
                if (rule is Wrong)
                {
                    if (!wrongWords.Contains(words[i]))
                        wrongWords.Add(words[i]);
                }
                rule = null;
            }

            return wrongWords.ToArray();
        }

        public static string[] load(string path)
        {
            try
            {
                using (StreamReader sr = new StreamReader("path", System.Text.Encoding.UTF8))
                {
                    String line = sr.ReadToEnd();
                    string[] words = GetWords(line);
                    return words;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("The file could not be read:");
                return null;
            }
        }

        public static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }
        static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
    }
    class Rule // check từ chỉ bao gồm các chữ cái tiếng việt
    {
        public int index = 0; // vị trí của con trỏ trong từ 
        public static string[] VietNamChars = { "a", "à", "á", "ả", "ã", "ạ", "ă", "ắ", "ặ", "ẳ", "ằ", "ẵ", "â", "ầ", "ấ", "ẩ", "ẫ", "ậ", "b",
            "c", "d", "đ", "e", "è", "é", "ẻ", "ẽ", "ẹ", "ê", "ề", "ế", "ể", "ệ", "ễ", "g", "h", "i", "ì", "í", "ĩ", "ỉ", "ị", "k", "l", "m",
            "n", "o", "ò", "ó", "ỏ", "õ", "ọ", "ô", "ồ", "ố", "ổ", "ỗ", "ộ", "ơ", "ờ", "ớ", "ở", "ỡ", "ợ", "p", "q", "r", "s", "t", "u", "ù",
            "ú", "ủ", "ũ", "ụ", "ư", "ừ", "ứ", "ữ", "ử", "ự", "v", "x", "y", "ỳ", "ý", "ỷ", "ỹ", "ỵ" };

        public virtual Rule Check(string word)
        {
            int length = word.Length;
            if (length > 7)
            {
                return new Wrong();
            }

            if (int.TryParse(word, out int n))
            {
                return new Right();
            }

            for (int j = 0; j < length; j++)
            {
                if (!VietNamChars.Contains(word[j] + ""))
                {
                    return new Wrong();
                }

            }
            return new Rule1();
        }

        public static string removeSign(string text)
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


        public static string[] GetWords(string input)
        {
            MatchCollection matches = Regex.Matches(input, @"\b[\w']*\b");

            var words = from m in matches.Cast<Match>()
                        where !string.IsNullOrEmpty(m.Value)
                        select TrimSuffix(m.Value);

            return words.ToArray();
        }
        static string TrimSuffix(string word)
        {
            int apostropheLocation = word.IndexOf('\'');
            if (apostropheLocation != -1)
            {
                word = word.Substring(0, apostropheLocation);
            }

            return word;
        }
    }

    internal class Rule1 : Rule //check phụ âm đầu 
    {
        public static string[] singleConsonants = { "b", "c", "d", "đ", "g", "h", "k", "l", "m", "n", "r", "s", "t", "v", "x", "p", "q" };
        public override Rule Check(string word)
        {
            string firstLetter = word[this.index] + "";
            if (singleConsonants.Contains(firstLetter)) //Nếu chữ cái đầu là phụ âm => check cặp phụ âm
            {
                Rule2 rule2 = new Rule2();
                rule2.index = this.index + 1; //tăng vị trí con trỏ lên 1;
                return rule2;
            }
            else // Nếu chữ cái đầu ko là phụ âm -> check nguyên âm đầu - giu nguyen vi tri con tro
            {
                Rule3 rule3 = new Rule3();
                rule3.index = this.index;
                return rule3;
            }
        }

    }



    internal class Rule2 : Rule // check 2 phụ âm đầu
    {

        public static string[] pairConsonants = { "ch", "gh", "kh", "nh", "ng", "ph", "th", "tr", "gi", "qu" };
        public override Rule Check(string word)
        {
            if (word.Length > 3 && ("" + word[0] + word[1] + word[2]).Equals("ngh")) //nếu chữ cái đầu là ngh tăng 2 lần vị trí con trỏ và kiểm tra nguyên âm đầu => Rule3
            {
                Rule3 rule3 = new Rule3();
                rule3.index = this.index + 2; // tang con tro len 2
                return rule3;
            }
            string firstPairLetter = word.Substring(0, 2);

            if (pairConsonants.Contains(firstPairLetter)) // la cap phu am => chech nguyen am giua goi Rule3 tăng con trỏ
            {
                Rule3 rule3 = new Rule3();
                rule3.index = this.index + 1; // tang con tro len 1
                return rule3;
            }

            if (Rule1.singleConsonants.Contains(word[1] + "")) // chữ cái thứ 2 là phụ âm =>  phụ âm ko hợp lệ 
            {
                return new Wrong();
            }
            // chữ cái thứ 2 là nguyên ầm = > chech nguyen am giua goi Rule3 - khong tang con tro
            else
            {
                Rule3 rule3 = new Rule3();
                rule3.index = this.index;
                return rule3;
            }
        }

    }

    internal class Rule3 : Rule //kiểm tra nguyên âm đầu tiên trong từ
    {
        public static string[] singleVowel = { "a", "à", "á", "ả", "ã", "ạ", "ă", "ắ", "ặ", "ẳ", "ằ", "ẵ", "â", "ầ", "ấ", "ẩ", "ẫ", "ậ", "e", "è", "é", "ẻ", "ẽ", "ẹ",
            "ê", "ề", "ế", "ể", "ệ", "ễ", "i", "ì", "í", "ĩ", "ỉ", "ị", "o", "ò", "ó", "ỏ", "õ", "ọ", "ô", "ồ", "ố", "ổ", "ỗ", "ộ", "ơ", "ờ", "ớ", "ở", "ỡ", "ợ",
            "u", "ù", "ú", "ủ", "ũ", "ụ", "ừ", "ứ", "ữ", "ử", "ự", "ư", "y", "ỳ", "ý", "ỷ", "ỹ", "ỵ" };
        public override Rule Check(string word)
        {
            int position = this.index;
            string currentSingleVowel = word[position] + "";
            if (singleVowel.Contains(currentSingleVowel)) //nếu nó là nguyên âm
            {
                // Bắt buộc thêm nguyên âm cuối, hoặc phụ âm cuối: â
                currentSingleVowel = Rule.removeSign(currentSingleVowel);
                if (currentSingleVowel.Equals("â") && (position + 1 == word.Length))
                    return new Wrong();

                // Bắt buộc thêm phụ âm cuối: ă
                if (currentSingleVowel.Equals("ă") && ((position + 1 == word.Length) || (singleVowel.Contains(word[position + 1] + ""))))
                    return new Wrong();

                // Nếu nguyên âm ở cuối thì
                if ((position + 1 == word.Length))
                    return new Right();

                Rule4 rule4 = new Rule4();
                rule4.index = position + 1; //tăng con trỏ lên 1
                return rule4;
            }
            else //không phải nguyên âm
            {
                return new Wrong();
            }
        }
    }

    internal class Rule4 : Rule //kiểm tra có phải là nguyên âm đôi hay không
    {
        public static string[] doubleVowel = { "ai", "ao", "au", "âu", "ay", "ây", "eo", "êu", "ia", "iê", "yê", "iu", "oa", "oă", "oe",
            "oi", "ôi", "ơi", "oo", "ua", "uă", "uâ", "ưa", "uê", "ui", "ưi", "uo", "uô", "uơ", "ươ", "ưu", "uy" };

        public static string[] doubleVowel1 = { "iê", "uâ", "ươ", "yê" }; // Xâu Bắt buộc thêm nguyên âm cuối, hoặc phụ âm cuối
        public static string[] doubleVowel2 = { "oă", "oo", "uă" }; // Xâu Bắt buộc thêm phụ âm cuối
        public static string[] doubleVowel3 = { "ai", "ao", "au", "âu", "ay", "ây", "eo", "êu", "ia", "iu", "oi", "ôi", "ơi", "ưa", "ui", "ưi", "ưu", }; //nguyên âm ghép không thêm được phần âm cuối 
        public static string[] doubleVowel4 = { "oa", "oe", "uê", "uy" }; //có thể đứng tự do một mình hoặc thêm âm đầu, cuối, hoặc cả đầu lẫn cuối
        public override Rule Check(string word)
        {
            string temp = Rule.removeSign(word);
            int position = this.index;
            string currentDoubleVowel = temp.Substring(position - 1, 2);

            if (Rule1.singleConsonants.Contains(word[position] + "")) // nếu chữ cái tiếp theo là phụ âm - kiem tra phu am cuoi - khong tang con tro
            {
                Rule6 rule6 = new Rule6();
                rule6.index = position;
                return rule6;
            }

            //new la nguyen am check nguyen am doi
            if (doubleVowel.Contains(currentDoubleVowel)) //Nếu nó là nguyên âm đôi
            {
                // Kiểm tra nếu xâu là doubleVowel1  (sau nó có nguyên âm cuối, hoặc phụ âm cuối)
                if (doubleVowel1.Contains(currentDoubleVowel))
                {
                    if (position + 1 == word.Length) // và vị trí ở cuối từ
                        return new Wrong();
                    if (!checkSign(word, temp, position))
                    {
                        return new Wrong();
                    }

                }

                // Kiểm tra nếu xâu là doubleVowel2 và vị trí ở cuối từ hoặc sau nó là một nguyên âm
                if (doubleVowel2.Contains(currentDoubleVowel))
                {
                    if (position + 1 == word.Length) // nếu ở cuối từ
                        return new Wrong();
                    if (Rule3.singleVowel.Contains(word[position + 1] + "")) // nếu sau đó là 1 nguyên âm
                    {
                        return new Wrong();
                    }
                    if (!checkSign(word, temp, position))
                    {
                        return new Wrong();
                    }
                    //Kiểm tra phụ âm cuối
                    Rule6 rule6 = new Rule6();
                    rule6.index = position + 1;
                    return rule6;
                }

                // Kiểm tra nếu xâu là doubleVowel3 và vị trí ở cuối từ
                if (doubleVowel3.Contains(currentDoubleVowel))
                {
                    if (position + 1 != word.Length) //nếu ko là ở vị trí cuối từ
                    {
                        return new Wrong();
                    }
                    if (!checkSign(word, temp, position - 1))//nếu đặt sai dấu
                    {
                        return new Wrong();
                    }
                    return new Right();

                }

                // Kiểm tra nếu xâu là doubleVowel4 vị trí ở cuối từ hoặc đứng một mình
                if (doubleVowel4.Contains(currentDoubleVowel))
                {
                    if (position + 1 == word.Length)
                    {
                        if ((!word.Equals(temp)) && (temp[position] != word[position]) && (temp[position - 1] != word[position - 1]))
                            return new Wrong();
                        return new Right();
                    }
                }
                Rule5 rule5 = new Rule5();
                rule5.index = position + 1;
                return rule5;


            }
            else
            {
                return new Wrong();
            }

        }
        public static bool checkSign(string str1, string str2, int pos)
        {
            if (str1.Equals(str2))
                return true;
            int len = str1.Length;
            if (!str1.Substring(0, pos).Equals(str2.Substring(0, pos)))
            {
                return false;
            }
            if (!str1.Substring(pos + 1, len - pos - 1).Equals(str2.Substring(pos + 1, len - pos - 1)))
            {
                return false;
            }
            return true;
        }

    }
    internal class Rule5 : Rule //kiểm tra có phải là nguyên âm tam hay không
    {
        public static string[] tripleVowel = { "iêu", "yêu", "oai", "oao", "oay", "oeo", "uao", "uây", "uôi", "ươi", "ươu", "uya", "uyê", "uyu" };
        public static string[] tripleVowel1 = { "uyê" }; // Xâu Bắt buộc thêm phụ âm cuối
        public static string[] tripleVowel2 = { "iêu", "yêu", "oai", "oao", "oay", "oeo", "uai", "uây", "uôi", "ươi", "ươu", "uya", "uyu", "uay" }; // nguyên âm tam không thêm được phần âm cuối 
        public override Rule Check(string word)
        {
            string temp = Rule.removeSign(word);
            int position = this.index;
            if (Rule1.singleConsonants.Contains(word[position] + "")) // nếu chữ cái tiếp theo là phụ âm - kiem tra phu am cuoi - khong tang con tro
            {
                if (!Rule4.checkSign(word, temp, position - 1))
                {
                    return new Wrong();
                }

                Rule6 rule6 = new Rule6();
                rule6.index = position;
                return rule6;
            }

            string currentTripleVowel = temp.Substring(position - 2, 3);
            if (tripleVowel.Contains(currentTripleVowel)) // nếu là nguyên âm tam
            {
                // Kiểm tra nếu xâu là tripleVowel1 và vị trí ở cuối từ hoặc sau nó là một nguyên âm
                if (tripleVowel1.Contains(currentTripleVowel))
                {
                    if (position + 1 == word.Length)
                        return new Wrong();

                    if (Rule3.singleVowel.Contains(word[position + 1] + ""))
                        return new Wrong();

                    if (!Rule4.checkSign(word, temp, position))
                    {
                        return new Wrong();
                    }
                }


                // Kiểm tra nếu xâu là tripleVowel2 và vị trí ở cuối từ
                if (tripleVowel2.Contains(currentTripleVowel))
                {
                    if ((position + 1 == word.Length)) //nếu là ở cuối từ
                    {
                        if (!Rule4.checkSign(word, temp, position - 1))
                        {
                            return new Wrong();
                        }
                        return new Right();
                    }
                    else
                    {
                        return new Wrong();
                    }
                }


                //Kiêm tra phu am cuối - tang con tro len 1
                Rule6 rule6 = new Rule6();
                rule6.index = position + 1;
                return rule6;
            } // khong la nguyen am tam
            else
            {
                return new Wrong();
            }


        }
    }

    internal class Rule6 : Rule //kiểm tra phụ âm cuối đơn
    {
        public static string[] SingleEndConsonant = { "c", "m", "n", "t", "p" };

        public override Rule Check(string word)
        {
            //Check phụ âm cuối đơn
            if (this.index == (word.Length - 1))
            {
                if (!SingleEndConsonant.Contains(word[this.index] + ""))
                    return new Wrong();
            }
            else
            {   //Check phụ âm cuối đôi
                //Tăng con trỏ lên 1
                Rule7 rule7 = new Rule7();
                rule7.index = this.index + 1;
                return rule7;
            }
            return new Right();
        }
    }

    internal class Rule7 : Rule
    {
        public static string[] PairEndConsonant = { "ch", "ng", "nh" };

        public override Rule Check(string word)
        {
            //Phụ âm cuối có độ dài tối đa là 2
            if (this.index < (word.Length - 1))
                return new Wrong();
            if (!PairEndConsonant.Contains(word.Substring(index - 1, 2)))
                return new Wrong();
            return new Right();
        }
    }

    internal class Right : Rule
    {
        public override Rule Check(string word)

        {
            this.index = word.Length;
            return this; //lặp lại cho đến hết từ, vì từ đã được xác định đúng chính tả
        }

    }

    internal class Wrong : Rule
    {
        public override Rule Check(string word)
        {
            this.index = word.Length;
            return this; //lặp lại cho đến hết từ, vì từ đã được xác định sai chính tả
        }

    }
}
