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
    class serwer
    {

        static async Task serverTask()
        {
            TcpListener server = new TcpListener(IPAddress.Any, 2048);
            server.Start();
            while (true)
            {
                TcpClient client = await server.AcceptTcpClientAsync();
                byte[] buffer = new byte[1024];
                client.GetStream().ReadAsync(buffer, 0, buffer.Length).ContinueWith(
                async (t) =>
                {
                    int i = t.Result;
                    while (true)
                    {
                        client.GetStream().WriteAsync(buffer, 0, i);
                        if (buffer[0] == 1) Console.WriteLine("Serwer - Otrzymalem wiadomosc");
                        i = await client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                    }
                });
            }
        }
        static void Main(string[] args)
        {

            Task s = serverTask();
            client c;
            client.Run(1);
            client.Run(2);

            Task[] array = { s };

            Thread.Sleep(100);
            client.Cancel(1);
            client.Cancel(2);
            Task.WaitAll(array);


        }

    }
    class client
    {

        static CancellationTokenSource[] tokenSource = new CancellationTokenSource[1024];




        static async Task ClientTask(CancellationToken ct)
        {

            TcpClient Client = new TcpClient();
            Client.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 2048));
            byte[] buffer = new byte[1024];
            buffer[0] = 1;
            Client.GetStream().WriteAsync(buffer, 0, buffer.Length).ContinueWith(
                async (t) =>
                {
                    int i = 0;
                    while (!ct.IsCancellationRequested)
                    {
                        i = await Client.GetStream().ReadAsync(buffer, 0, buffer.Length);
                        Console.WriteLine("Client - Wysylam wiadomosc");
                        Client.GetStream().WriteAsync(buffer, 0, i);
                    }



                }, ct);



        }

        public static async void Run(int number)
        {

            tokenSource[number] = new CancellationTokenSource();
            CancellationToken ct;
            ct = tokenSource[number].Token;
            ClientTask(ct);



        }

        public static void Cancel(int number)
        {
            tokenSource[number].Cancel();
        }
    }
}