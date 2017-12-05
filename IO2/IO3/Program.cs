﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IO3
{
    public class MatMulCalculatorCompletedEventArgs : AsyncCompletedEventArgs
    {
        int size;
        double[] matrix;
        public MatMulCalculatorCompletedEventArgs(double[] matrix, int size, Exception ex, bool canceled, object userState) : base(ex, canceled, userState)
        {
            this.Size = size;
            this.matrix = matrix;

        }

        public int Size { get => size; set => size = value; }

        public class MatMulCalculator
        {
            public delegate void MatMulCalculatorCompletedEventHandler(object sender, MatMulCalculatorCompletedEventArgs e);
            private delegate void WorkerEventHandler(double[] mat1, double[] mat2, int size, AsyncOperation asyncOp);
            public event MatMulCalculatorCompletedEventHandler MatMulCalculatorCompleted;
            private SendOrPostCallback onCompletedCallback;
            private HybridDictionary tasks = new HybridDictionary();

            public MatMulCalculator()
            {
                onCompletedCallback = new SendOrPostCallback(CalculateCompleted);
            }

            private void CalculateCompleted(object operationState)
            {
                MatMulCalculatorCompletedEventArgs e = operationState as MatMulCalculatorCompletedEventArgs;
                if (MatMulCalculatorCompleted != null)
                {
                    MatMulCalculatorCompleted(this, e);
                }
            }
            void Completion(int size, double[] mat, Exception ex, bool cancelled, AsyncOperation ao)
            {
                if (!cancelled)
                {
                    lock (tasks.SyncRoot)
                    {
                        tasks.Remove(ao.UserSuppliedState);
                    }
                }
                MatMulCalculatorCompletedEventArgs e = new MatMulCalculatorCompletedEventArgs(mat, size, ex, cancelled, ao.UserSuppliedState);
                ao.PostOperationCompleted(onCompletedCallback, e);


            }
            bool TaskCancelled(object taskID)
            {
                return (tasks[taskID] == null);
            }
            void CalculateWorker(double[] mat1, double[] mat2, int size, AsyncOperation asyncOp)
            {
                Exception e = null;

                double[] results = MatMul(mat1, mat2, size);

                this.Completion(size, results, e, TaskCancelled(asyncOp.UserSuppliedState), asyncOp);
            }

            double[] MatMul(double[] mat1, double[] mat2, int size)
            {
                double[,] mata = new double[size, size];
                double[,] matb = new double[size, size];
                int counter = 0;
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        mata[i, j] = counter;
                        matb[i, j] = counter++;
                    }
                }
                counter = 0;
                double[] result = new double[size * size];
                for (int i = 0; i < size; i++)
                {
                    for (int j = 0; j < size; j++)
                    {
                        double w = 0;
                        for (int k = 0; k < size; k++)
                            w += mata[i, k] * matb[k, j];
                        result[counter] = w;
                    }
                }
                return result;

            }
            public virtual void MatMulAsync(double[] mat1, double[] mat2, int size, object taskId)
            {
                AsyncOperation asyncOp =
                AsyncOperationManager.CreateOperation(taskId);

                lock (tasks.SyncRoot)
                {
                    if (tasks.Contains(taskId))
                    {
                        throw new ArgumentException(
                         "Task ID parameter must be unique",
                         "taskId");
                    }
                    tasks[taskId] = asyncOp;
                }
                WorkerEventHandler workerDelegate = new WorkerEventHandler(CalculateWorker);
                workerDelegate.BeginInvoke(mat1, mat2, size, asyncOp, null, null);



            }
            public void CancelAsync(object taskId)
            {
                AsyncOperation asyncOp = tasks[taskId] as AsyncOperation;
                if (asyncOp != null)
                {
                    lock (tasks.SyncRoot)
                    {
                        tasks.Remove(taskId);
                    }
                }
            }

            void funkcja(object sender, MatMulCalculatorCompletedEventArgs e)
            {
                Console.WriteLine(e.size);
            }
            static void Main(string[] args)
            {
                MatMulCalculator c = new MatMulCalculator();
                double[] a = { 2, 9, 7, 2 };
                double[] b = { 2, 2,3, 7 };
                c.MatMulAsync(a, b, 2, 1);
                c.MatMulCalculatorCompleted += c.funkcja;

                Thread.Sleep(10000);
            }
        }

}





}