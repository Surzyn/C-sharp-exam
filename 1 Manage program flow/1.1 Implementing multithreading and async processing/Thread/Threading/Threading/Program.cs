using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Threading
{
    class Program
    {
        [ThreadStatic]
        public static int number = 10;

        public static ThreadLocal<int> localNumber = new ThreadLocal<int>(() =>
        {
            return 10;
        });

        static void Main(string[] args)
        {
            Thread t = new Thread(new ThreadStart(ThreadMethod));
            t.Start();

            for (int i = 0; i < 5; i++)
            {
                number += 1;
                localNumber.Value += 1;
                Console.WriteLine("Main thread {0} number {1} localNumber {2}", i, number, localNumber);
                Thread.Sleep(500);
            }


            t.Join();

            ThreadPool.QueueUserWorkItem((s) =>
            {
                Console.WriteLine("Thread pool");
            });


            Task<int> task = Task.Run(() =>
            {
                return 42;
            }).ContinueWith((i)=>
            {
                return i.Result * 2;
            },TaskContinuationOptions.OnlyOnFaulted);

            Console.WriteLine(task.Result);

            Console.ReadKey();
        }


        public static void ThreadMethod()
        {
            for (int i = 0; i < 5; i++)
            {
                number += 1;
                localNumber.Value += 1;
                Console.WriteLine("Main thread {0} number {1} localNumber {2}", i, number, localNumber);
                Thread.Sleep(1000);
            }
        }
    }
}
