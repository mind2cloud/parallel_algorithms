using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace IntegralTrapecia
{
    class Program
    {
        static double ParallelSquare(SeparateTrapeze separateTrapeze)
        {
            double[] TotalSum = new double[separateTrapeze.n];
            double x1 = separateTrapeze.x1;
            double x2 = separateTrapeze.x2;
            double dx = (x2 - x1) / separateTrapeze.n;
            List<SeparateTrapeze> s = new List<SeparateTrapeze>(separateTrapeze.n);

            for (int i = 0; i < separateTrapeze.n; i++)
            {
                s.Add(new SeparateTrapeze(1, (x1 + (dx * i)), (x1 + ((i + 1) * dx))));
                TotalSum[i] = (Math.Pow(2, s[i].x1) + Math.Pow(2, s[i].x2)) * dx / 2;
            }
            return TotalSum.Sum();
        }

        // Вариант №8: 2^x, особых точек нет
        static void Main(string[] args)
        {
            double a, b;
            int steps, tasks;

            do
            {
                Console.Write("Введите начало отрезка интегрирования: ");
                a = double.Parse(Console.ReadLine());

                Console.Write("Введите конца отрезка интегрирования: ");
                b = double.Parse(Console.ReadLine());

                Console.Write("Введите количество шагов: ");
                steps = int.Parse(Console.ReadLine());

                Console.Write("Введите количество потоков: ");
                tasks = int.Parse(Console.ReadLine());
                Console.WriteLine();

                if ((tasks > 0) && (a < b)) {
                    break;
                } else {
                    Console.WriteLine("Введённые данные некорректны");
                }
            } while (true);

            //начинаем считать интеграл
            Stopwatch sw = new Stopwatch();
            sw.Start();

            double dy = (b - a) / tasks;
            double[] y = new double[steps];
            Task<double>[] task1 = new Task<double>[tasks];
            List<SeparateTrapeze> trapeze = new List<SeparateTrapeze>(tasks);
            double[] FirstSum = new double[tasks];

            for (int i = 0; i < tasks; i++)
            {
                trapeze.Add(new SeparateTrapeze(steps, a + (i * dy), a + ((i + 1) * dy)));
                task1[i] = new Task<double>(x => (ParallelSquare((SeparateTrapeze)x)), trapeze[i]);
                task1[i].Start();
                FirstSum[i] = task1[i].Result;
            }

            sw.Stop();
            Console.WriteLine("Время выполнения: {0}", sw.ElapsedMilliseconds.ToString());
            Console.WriteLine("Значение интеграла: {0}", FirstSum.Sum());
            Console.ReadKey();
        }
    }

    public class SeparateTrapeze
    {
        public int n;
        public double x1;
        public double x2;
        public SeparateTrapeze(int n, double x1, double x2)
        {
            this.n = n;
            this.x1 = x1;
            this.x2 = x2;
        }

    }
}
