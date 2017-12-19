using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOLab
{
    class Zad1_4
    {
        public static void zad1()
        {
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 510 });
            ThreadPool.QueueUserWorkItem(ThreadProc, new object[] { 500 });
            Thread.Sleep(1000);
        }
        public static void ThreadProc(object stateinfo)
        {
            Thread.Sleep(Convert.ToInt32(((object[])stateinfo)[0]));
            Console.WriteLine("Thread wait:" + Convert.ToInt32(((object[])stateinfo)[0]));

        }

        public static void zad2()
        {
            ThreadPool.QueueUserWorkItem(Server);
            ThreadPool.QueueUserWorkItem(Client, new object[] { "Halo" });
            ThreadPool.QueueUserWorkItem(Client, new object[] { "Zaczynamy" });
            Console.ReadKey();

        }

        private static Object thisLock = new Object();
        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            lock (thisLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
        }
        static void Server(Object stateInfo)
        {
            TcpClient client = (TcpClient)stateInfo;
            while (true)
            {
                Console.WriteLine("Waiting for a connection.....");
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                writeConsoleMessage("(Serwer)Otrzymalem wiadomosc: " + System.Text.Encoding.Default.GetString(buffer, 0, buffer.Length), ConsoleColor.Green);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
        }
        static void Client(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));

            while (true)
            {
                byte[] message = new ASCIIEncoding().GetBytes("Wiadomosc");
                client.GetStream().Write(message, 0, message.Length);
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);
                writeConsoleMessage("(Client)Otrzymalem wiadomosc: " + System.Text.Encoding.Default.GetString(buffer, 0, buffer.Length), ConsoleColor.Red);
            }
        }

        static void Main(string[] args)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            ThreadPool.QueueUserWorkItem(Client);
            ThreadPool.QueueUserWorkItem(Client);
            ThreadPool.QueueUserWorkItem(Client);
            while (true)
            {
                TcpClient client = server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(Server, client);
            }
        }
    }
}

