using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IOLab
{
    class Value
    {
        public long result = 0;
    }
    class Info
    {
        public int poczatek;
        public int koniec;
        public int[] tab;
        public object locker;
        public Value result;
        public WaitHandle handle;
        public Info(int poczatek, int koniec, int[] tab, object locker, Value result, WaitHandle handle)
        {
            this.poczatek = poczatek;
            this.koniec = koniec;
            this.tab = tab;
            this.locker = locker;
            this.result = result;
            this.handle = handle;
        }
    }
    class Zd5
    {
        private static int[] tab;
        private static int suma;
        private static Random generator;
        private object locker = new object();
        public int n = 1000;
        public int segment = 100;
        public Value result = new Value();

        public Zd5()
        {
            generator = new Random();
            tab = new int[n];
            for (int i = 0; i < tab.Length; i++)
            {
                tab[i] = generator.Next();
            }
        }
        static void ThreadProc(Object stateInfo)
        {
            Info inf = (Info)stateInfo;
            long result = 0;
            for (int i = inf.poczatek; i <= inf.koniec; i++)
            {
                result += inf.tab[i];
            }
            lock (inf.locker)
            {
                inf.result.result += result;
            }
            AutoResetEvent are = (AutoResetEvent)inf.handle;
            are.Set();


        }
        static void Main(string[] args)
        {
            Value result = new Value();
            Zd5 sum = new Zd5();
            int watki = sum.n / sum.segment;
            WaitHandle[] waitHandles = new WaitHandle[watki];
            for (int i = 0; i < watki; i++)
            {
                waitHandles[i] = new AutoResetEvent(false);
            }
            for (int i = 0; i < watki; i++)
            {
                Info inf = new Info(i * sum.segment, ((i + 1) * sum.segment) - 1, tab, sum.locker, result, waitHandles[i]);
                if (((i + 1) * sum.segment) > sum.n) inf.koniec = sum.n - 1;
                ThreadPool.QueueUserWorkItem(new WaitCallback(ThreadProc), inf);
            }
            WaitHandle.WaitAll(waitHandles);
            Console.Write("Suma:" + result.result);
            Thread.Sleep(6000); 
        }
    }
}

  


        
   

 
