using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace IO
{
    class Program
    {
        
        public static void zad1()
        {
            ThreadPool.QueueUserWorkItem(ThreadProc,new object[]{ 510 });
            ThreadPool.QueueUserWorkItem(ThreadProc,new object[]{ 500 });
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
            ThreadPool.QueueUserWorkItem(Client,new object[]{ "Halo" });
            ThreadPool.QueueUserWorkItem(Client, new object[] { "Zaczynamy" });
            Console.ReadKey();

        }
        public static void Client(object stateinfo)
        {
            string msg = Convert.ToString(((object[])stateinfo)[0]);
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] message = new ASCIIEncoding().GetBytes(msg);
            client.GetStream().Write(message, 0, message.Length);
            Console.WriteLine(" Client Sent: {0}", msg);

            Byte[] data = new Byte[256];
            String responseData = String.Empty;
            Int32 bytes = client.GetStream().Read(data, 0, data.Length);
            responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
            Console.WriteLine("Client Received: {0}", responseData);
        }
        public static void Server(object stateinfo)
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            Console.WriteLine("The server is running at port 2048...");
            Console.WriteLine("The local End point is  :" +
                              server.LocalEndpoint);
            while (true)
            {
                Console.WriteLine("Waiting for a connection.....");
                TcpClient client = server.AcceptTcpClient();
                Console.WriteLine("Connected");
                byte[] buffer = new byte[1024];
                NetworkStream stream = client.GetStream();
                int i;
                while ((i = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string data = System.Text.Encoding.ASCII.GetString(buffer, 0, i);
                    Console.WriteLine(" Server Received: {0}", data);

                    byte[] msg = System.Text.Encoding.ASCII.GetBytes(data);
                    client.GetStream().Write(msg, 0, msg.Length);
                    Console.WriteLine("Server Sent: {0}", data);
                }
                client.Close();
                stream.Close();
            }
            
        }


        public static void zad4()
        {
            TcpListener Server = new TcpListener(IPAddress.Any, 2048);
            Server.Start();
            ThreadPool.QueueUserWorkItem(client);
            ThreadPool.QueueUserWorkItem(client);
            ThreadPool.QueueUserWorkItem(client);
            while (true)
            {
                TcpClient client = Server.AcceptTcpClient();
                ThreadPool.QueueUserWorkItem(server, client);
            }
        }
        private static Object thisLock = new Object();
        static void writeConsoleMessage(string message, ConsoleColor color)
        {
            message = message.Replace("\0", "");
            lock (thisLock)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ResetColor();
            }
            Thread.Sleep(200); // dodane tylko po to aby zdążyć zauważyć co sie dzieje w konsoli 
        }
        static void server(Object stateInfo)
        {
            TcpClient client = (TcpClient)stateInfo;
            while (true)
            {
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, 1024);
                writeConsoleMessage("Serwer! Otrzymalem wiadomosc: " + System.Text.Encoding.Default.GetString(buffer, 0, buffer.Length), ConsoleColor.Green);
                client.GetStream().Write(buffer, 0, buffer.Length);
            }
        }
        static void client(Object stateInfo)
        {
            TcpClient client = new TcpClient();
            client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));

            while (true)
            {
                byte[] message = new ASCIIEncoding().GetBytes("Wiadomosc");
                client.GetStream().Write(message, 0, message.Length);
                byte[] buffer = new byte[1024];
                client.GetStream().Read(buffer, 0, buffer.Length);
                writeConsoleMessage("Client! Otrzymalem wiadomosc: " + System.Text.Encoding.Default.GetString(buffer, 0, buffer.Length), ConsoleColor.Red);
            }
        }


        static void Main(string[] args)
        {
            zad1();
        }
    }
}
