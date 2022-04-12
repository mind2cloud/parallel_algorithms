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
        //Перемножение матриц обычным способом
        static double[,] usualMultiply(double[,] A, double[,] B, int width, int height, int n)
        {
            double[,] result = new double[width, height];

            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    for (int s = 0; s < n; s++)
                    {
                        result[i, j] += A[i, s] * B[s, j];
                    }
                }
            }
            return result;
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

            int i1 = firstMatrixFromCsv.Length;
            int n1 = 0;
            int i2 = secondMatrixFromCsv.Length;
            int n2 = 0;

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

            double[,] firstMatrix = new double[i1, n1];
            double[,] secondMatrix = new double[i2, n2];

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


                result = usualMultiply(firstMatrix, secondMatrix, weight, height, n);
                //result = parallelMultiply(firstMatrix, secondMatrix, weight, height, n);

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
