using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace IO4
{
    class Program
    {
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



        static void Main(string[] args)
        {
           
          
        }
    }
}

