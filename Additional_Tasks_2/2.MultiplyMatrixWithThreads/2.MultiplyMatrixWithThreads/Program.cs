using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SecondLaboratoryWork
{
    class Program
    {
        static int i1 = 1000;
        static int n1 = 0;
        static int i2 = 1000;
        static int n2 = 0;
        static double[,] firstMatrix = new double[i1, n1];
        static double[,] secondMatrix = new double[i2, n2];

        //Перемножение матриц c помощью Threads
        public static void usualMultiply(object x)
        {
            int n = (int) x;
            double[,] result = new double[i1, n2];

            for (int i = 0; i < i1; i++)
            {
                for (int j = 0; j < n2; j++)
                {
                    for (int s = 0; s < n; s++)
                    {
                        result[i, j] += firstMatrix[i, s] * secondMatrix[s, j];
                    }
                }
            }
        }

        //Перемножение матриц параллельным способом
        static double[,] parallelMultiply(double[,] A, double[,] B, int width, int height, int n)
        {
            double[,] result = new double[width, height];

            Parallel.For(0, width, (i) =>
            {
                for (int j = 0; j < height; j++)
                {
                    for (int s = 0; s < n; s++)
                    {
                        result[i, j] += A[i, s] * B[s, j];
                    }
                }
            });
            return result;
        }

        static void Main(string[] args)
        {
            Stopwatch sWatchOrdinary = new Stopwatch();
            sWatchOrdinary.Start();
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en");

            var firstMatrixFromCsv = File.ReadAllLines(@"matrix.csv");
            var secondMatrixFromCsv = File.ReadAllLines(@"matrix.csv");

            i1 = firstMatrixFromCsv.Length;
            n1 = 0;
            i2 = secondMatrixFromCsv.Length;
            n2 = 0;

            foreach (string line in firstMatrixFromCsv)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                    j++;
                n1 = j;
            }
            foreach (string line in secondMatrixFromCsv)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                    j++;
                n2 = j;
            }

            int r = 0;
            foreach (string line in firstMatrixFromCsv)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    firstMatrix[r, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++r;
            }
            r--;

            int r2 = 0;
            foreach (string line in secondMatrixFromCsv)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    secondMatrix[r2, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++r2;
            }

            int weight = i1;
            int height = n2;
            int n = n1;
            double[,] result = new double[weight, height];
            int numberOfThreads = 100;

            //result = usualMultiply(firstMatrix, secondMatrix, weight, height, n);
            //result = parallelMultiply(firstMatrix, secondMatrix, weight, height, n);

            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();

            Thread[] threads = new Thread[numberOfThreads];
            for (int i = 0; i < numberOfThreads; i++)
            {
                threads[i] = new Thread(new ParameterizedThreadStart(usualMultiply));
                threads[i].Start(i);
            }
            for (int i = 0; i < numberOfThreads; i++)
            {
                while (threads[i].ThreadState != System.Threading.ThreadState.Stopped)
                {
                }
            }

            sWatch.Stop();

            StreamWriter sw = new StreamWriter(@"answer.csv");
            for (int i = 0; i < weight; i++)
            {
                for (int j = 0; j < height - 1; j++)
                    sw.Write(result[i, j] + ",");
                sw.Write(result[i, height - 1]);
                sw.WriteLine();
            }
            sw.Close();


            sWatchOrdinary.Stop();
            Console.WriteLine("Multiplication time: {0} mc", sWatchOrdinary.ElapsedMilliseconds.ToString());
            Console.ReadKey();
        }
    }
}
