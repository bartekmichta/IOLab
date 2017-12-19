using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace IOLab
{
    class Zd11
    {
        static BackgroundWorker bworker = new BackgroundWorker();
        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            bworker.DoWork += DoWork;
            bworker.ProgressChanged += ProcessChanged;
            bworker.WorkerReportsProgress = true;
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                bworker.RunWorkerAsync(client);
            }
        }

        private static void DoWork(object sender, DoWorkEventArgs e)
        {
            TcpClient client = (TcpClient)e.Argument;
            int counter = 0;
            while (counter < 100)
            {

                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                client.GetStream().Write(buffer, 0, buffer.Length);

                counter += 1;
                bworker.ReportProgress(counter);

            }
        }
        private static void ProcessChanged(object sender, ProgressChangedEventArgs e)
        {
            Console.WriteLine("wiadomosci: " + e.ProgressPercentage, ConsoleColor.Green);
        }
    }
}
