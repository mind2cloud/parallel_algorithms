using System;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.Linq;

namespace FunctionExtremums
{
    class MainClass
    {
        const double dx = 1e-4;
        static double leftBorder = 0;
        static double rightBorder = 0;
        static int numberOfThreads = 0;
        static int[] extremums;

        static void findExtremum(double a, double b, int numberOfThread)
        {
            int numberOfExtremums = 0;
            for (double i = a + dx; i < b; i += dx)
            {
                // Если производные разных знаков - экстремум
                if (findDerivative(i - dx) * findDerivative(i) < 0)
                {
                    numberOfExtremums++;
                }
            }
            extremums[numberOfThread] = numberOfExtremums;
        }

        static double findDerivative(double x)
        {
            return (function(x + dx) - function(x)) / dx;
        }


        static double function(double x)
        {
            return Math.Cos(2 * x) + Math.Sin(3 * x);
        }

        static void separateFunction(object index)
        {
            int i = (int)index;
            double step = (rightBorder - leftBorder) / numberOfThreads;
            double start = 0, finish = 0;

            start = leftBorder + i * step - dx;
            start += dx;
            finish = start + step;

            findExtremum(start, finish, i);
        }


        static void Main(string[] args)
        {
            Console.Write("Введите начало отрезка: ");
            leftBorder = double.Parse(Console.ReadLine());

            Console.Write("Введите конец отрезка: ");
            rightBorder = double.Parse(Console.ReadLine());

            Console.Write("Введите количество потоков: ");
            numberOfThreads = int.Parse(Console.ReadLine());
            Console.WriteLine();

            extremums = new int[numberOfThreads];

            Thread[] threads = new Thread[numberOfThreads];
            Stopwatch sWatch = new Stopwatch();

            sWatch.Start();

            for (int i = 0; i < numberOfThreads; i += 1)
            {
                    threads[i] = new Thread(new ParameterizedThreadStart(separateFunction));
                    threads[i].Start(i);
            }

            sWatch.Stop();

            Console.WriteLine("Количество экстремумов: {0}", extremums.Sum());
            Console.WriteLine("Время выполнения: {0}", sWatch.ElapsedMilliseconds.ToString());
        }
    }
}