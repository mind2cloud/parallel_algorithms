using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace PrintNumbers
{
    class Program
    {
        // Создаем объект-заглушку
        static object locker = new object();

        static void printNumbersConsistentlyFirst()
        {
            lock (locker)
            {
                for (int i = 1; i < 11; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(300);
                }
            }

        }

        static void printNumbersConsistentlySecond()
        {
            lock (locker)
            {
                for (int i = 11; i < 21; i++)
                {
                    Console.WriteLine(i);
                    Thread.Sleep(305);
                }
            }

        }

        static void Main(string[] args)
        {
            List<Thread> threads = new List<Thread>();
            Thread firstThread = new Thread(new ThreadStart(printNumbersConsistentlyFirst));
            Thread secondThread = new Thread(new ThreadStart(printNumbersConsistentlySecond));

            threads.Add(firstThread);
            threads.Add(secondThread);

            foreach (var thread in threads)
            {
                thread.Start();
            }
        }
    }
}


//https://metanit.com/sharp/tutorial/11.4.php