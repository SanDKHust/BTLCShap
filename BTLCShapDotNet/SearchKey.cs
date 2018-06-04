using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BTLCShapDotNet
{
    class SearchKey
    {
        private string data;
        private int prioritize;
        public SearchKey(string data, int prioritize)
        {
            this.data = data;
            this.prioritize = prioritize;
        }
        public string getData()
        {
            return data;
        }
        public void setData(string data)
        {
            this.data = data;
        }
        public int getPrioritize()
        {
            return prioritize;
        }
        public void setPrioritize(int prioritize)
        {
            this.prioritize = prioritize;
        }
    }

    class Search
    {
        static void doSearch(SearchKey paragraph, string[] keysearch)
        {
            // Loại bỏ các từ tìm kiếm mà người dùng nhập trùng lặp.
            foreach (string key in keysearch.Distinct())
            {
                if (paragraph.getData().Contains(key))
                    paragraph.setPrioritize(paragraph.getPrioritize() + 1);
            }
            // Giả sử chỉ xuất ra những câu có xuất hiện ít nhất 2 key search trở lên.
            if (paragraph.getPrioritize() >= 2)
                Console.WriteLine(paragraph.getData());
        }
        // Tách lấy keysearch từ người dùng nhập vào
        static string[] getKeySearch(string search)
        {
            string[] keysearch = search.Split(' ');
            return keysearch;
        }
        static void SearchSentence(string search, string data)
        {
            string[] keysearch = getKeySearch(search);
            int prioritize = 0;
            string[] sentences = data.Split('\n');
            int len = sentences.Length;

            Thread thread1 = new Thread(() =>
            {
                for (int i = 0; i < len / 2; i++)
                {
                    SearchKey paragraph = new SearchKey(sentences[i], prioritize);
                    doSearch(paragraph, keysearch);
                }
            }
            );
            thread1.Start();
            // Thread2 tìm từ giữa đến hết.
            Thread thread2 = new Thread(() =>
            {
                for (int i = len / 2; i < len; i++)
                {
                    SearchKey paragraph = new SearchKey(sentences[i], prioritize);
                    doSearch(paragraph, keysearch);
                }
            }
            );
            thread2.Start();
            Console.ReadKey();
        }
    }
}
