using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO1
{
    class Sum
    {
        private static int[] tab;
        private static int n, suma;
        private static Random generator;

        delegate int DelegateType(int n);
        static DelegateType delegateName;

        public static int fib_i(int n)
        {
            int a, b, temp;
            if (n == 0) return 0;

            a = 0; b = 1;
            for (int i = 0; i < (n - 1); i++)
            {
                temp = a;
                a = b;
                b = temp;
                b += a;
            }
            return b;
        }
        public static int fib_r(int n)
        {
            if (n == 0) return 0;
            if (n == 1) return 1;
            return fib_r(n - 1) + fib_r(n - 2);
        }
        public static int silnia_r(int n)
        {
            if (n < 1)
                return 1;
            else
                return n * silnia_r(n - 1);
        }
        public static int silnia_i(int n)
        {
            int result = 1;
            for (int i = 1; i <= n; i++)
            {
                result *= i;
            }
            return result;
        }


        public Sum(int ile)
        {
            n = ile;
            generator = new Random();
            tab = new int[n];
            for(int i = 0; i < tab.Length; i++)
            {
                tab[i] = generator.Next();
            }
        }

        public static void amount(Object stateInfo)
        {
            lock (tab)
            {
                for (int i = 0; i < tab.Length; i++)
                {
                    suma = suma + tab[i];
                }
            }
            Console.WriteLine("Suma:"+suma);
        }

        public static void zad5()
        {
            Sum sum = new Sum(1000);
            ThreadPool.QueueUserWorkItem(new WaitCallback(amount));
            ThreadPool.QueueUserWorkItem(new WaitCallback(amount));
            ThreadPool.QueueUserWorkItem(new WaitCallback(amount));
            Thread.Sleep(1000);
            Console.ReadKey();
        }

        private static void myAsyncCallback(IAsyncResult ar)
        {
            object[] dane = (object[])ar.AsyncState;
            FileStream fs = (FileStream)dane[0];
            byte[] buffer = (byte[])dane[1];
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            fs.Close();
        }

        public static void zad6()
        {
            FileStream fs = new FileStream("file.txt", FileMode.Open);
            Byte[] buffer = new Byte[256];
            fs.BeginRead(buffer, 0, buffer.Length, myAsyncCallback, new object[] { fs, buffer });
            Console.ReadKey();
            
        }

        private static void zad_7()
        {
            FileStream fs = new FileStream("file.txt", FileMode.Open);
            Byte[] buffer = new Byte[256];
            var ar = fs.BeginRead(buffer, 0, buffer.Length, null, null);
            fs.EndRead(ar);
            Console.WriteLine(Encoding.ASCII.GetString(buffer));
            Console.ReadKey();

        }

        private static void zad_8()
        {
            delegateName = new DelegateType(fib_r);


            IAsyncResult ar = delegateName.BeginInvoke(40, null, null);
            Console.WriteLine("Fibonacci rekurencyjnie=" + delegateName.EndInvoke(ar));

            delegateName = fib_i;
            IAsyncResult ar1 = delegateName.BeginInvoke(40, null, null);
            Console.WriteLine("Fibonacci iteracyjnie=" + delegateName.EndInvoke(ar1));

            delegateName = silnia_r;
            IAsyncResult ar2 = delegateName.BeginInvoke(20, null, null);
            Console.WriteLine("Silnia rekurencyjnie=" + delegateName.EndInvoke(ar2));

            delegateName = silnia_i;
            IAsyncResult ar3 = delegateName.BeginInvoke(20, null, null);
            Console.WriteLine("Silnia rekurencyjnie=" + delegateName.EndInvoke(ar3));

            Console.ReadKey();
        }

        static void Main(string[] args)
        {
            zad_8();
        }
    }
}
