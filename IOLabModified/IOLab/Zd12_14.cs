using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace IOLab
{
    class Zd12_14
    {
        public struct TResultDataStructure
        {
            private int a, b;


            public int A { get => a; set => a = value; }
            public int B { get => b; set => b = value; }
        }
        public struct TResultDataStructure2
        {
            private string result;

            public string Result { get => result; set => result = value; }
        }
        public static async Task Download()
        {
            WebClient client = new WebClient();
            string html = await client.DownloadStringTaskAsync("http://www.feedforall.com/sample.xml");
            Console.WriteLine(html);
        }
        public static void zad14()
        {
            Console.WriteLine("BEGIN MAIN");
            Task task = Download();
            task.Wait();
            Console.ReadKey();
        }
        public void zad13()
        {
            bool Z2;
            Task.Run(() =>
            {
                Z2 = true;
            });
        }
    }
}
