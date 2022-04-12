using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;


namespace Monte_Carlo_Method
{
    class MainClass
    {
        public static double monteCarloMethod(int numberOfPoints)
        {
            double radius = 1.0;
            double counter = 0;
            double pi = 0;
            double[] x = new double[numberOfPoints];
            double[] y = new double[numberOfPoints];
            Random random = new Random();

            for (int i = 0; i < numberOfPoints; i++)
            {
                x[i] = random.NextDouble();
                y[i] = random.NextDouble();

                if (x[i] * x[i] + y[i] * y[i] < radius * radius)
                {
                    counter++;
                }
            }

            pi = 4 * counter / numberOfPoints;

            Console.WriteLine("count: {0}", counter);
            Console.WriteLine("n: {0}", numberOfPoints);
            Console.WriteLine("pi: {0}", pi);

            return pi;
        }

        public static void Main(string[] args)
        {
            int numberOfPoints = 0;
            int numberOfTasks = 0;
            double rezultPi = 0;

            Stopwatch sw = new Stopwatch();

            Console.Write("Enter Number of points: ");
            numberOfPoints = int.Parse(Console.ReadLine());
            Console.Write("Enter Number of tasks: ");
            numberOfTasks = int.Parse(Console.ReadLine());

            sw.Start();

            Task<double>[] task = new Task<double>[numberOfTasks];

            // Используем Parallel For:
            Parallel.For(0, numberOfTasks, (i) =>
            {

                task[i] = new Task<double>(x => monteCarloMethod(numberOfPoints), i);
                task[i].Start();
            });

            for (int i = 0; i < numberOfTasks; i += 1)
            {
                rezultPi = rezultPi + task[i].Result;
            }

            rezultPi = rezultPi / numberOfTasks;
            Console.WriteLine("Total Pi rezult is: {0}", rezultPi);
            sw.Stop();
            Console.WriteLine("Время выполнения: {0}", sw.ElapsedMilliseconds.ToString());
        }
    }
}