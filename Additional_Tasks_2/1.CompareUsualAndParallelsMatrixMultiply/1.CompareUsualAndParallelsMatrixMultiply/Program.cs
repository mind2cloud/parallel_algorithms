using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace CompareUsualAndParallelsMatrixMultiply
{
    class MainClass
    {
        //Перемножение матриц с помощью задач
        static void MultiplyWithTasks(int n, int task, double[,] firstMatrix, double[,] secondMatrix, int row, int col, int k, double[,] result)
        {
            int lt = (int)row / n + 1;
            int start = 0, finish = 0;

            start = task * lt;
            finish = start + lt;

            if (finish > row) finish = row;
            for (int i = start; i < finish; i++)
            {
                for (int j = 0; j < col; j++)
                {
                    for (int s = 0; s < k; s++)
                    {
                        result[i, j] += firstMatrix[i, s] * secondMatrix[s, j];
                    }
                }
            }
        }

        //Перемножение матриц с помощью ParallelFor
        static double[,] MultiplyWithParallelFor(double[,] firstMatrix, double[,] secondMatrix, int width, int height, int n)
        {
            double[,] result = new double[width, height];

            Stopwatch stopwatchFor = new Stopwatch();
            stopwatchFor.Start();

            Parallel.For(0, width, (i) =>
            {
                for (int j = 0; j < height; j++)
                {
                    for (int s = 0; s < n; s++)
                    {
                        result[i, j] += firstMatrix[i, s] * secondMatrix[s, j];
                    }
                }
            });

            stopwatchFor.Stop();
            Console.WriteLine("Время перемножения с помощью ParallelFor: {0} mc", stopwatchFor.ElapsedMilliseconds.ToString());

            return result;
        }

        static void Main(string[] args)
        {
            var lines = File.ReadAllLines(@"in.csv");
            double[,] matrix = new double[lines.Length, lines.Length];

            double[,] resultTasks = new double[lines.Length, lines.Length];
            double[,] resultParallelFor = new double[lines.Length, lines.Length];

            int i = 0;
            foreach (string line in lines)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    matrix[i, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    ++j;
                }
                ++i;
            }

            Console.Write("Введите количество задач: ");
            int numberOfTasks = Int32.Parse(Console.ReadLine());
            Console.WriteLine();

            Task[] tasks = new Task[numberOfTasks];
            int task = 0;
            Stopwatch stopwatchTasks = new Stopwatch();

            stopwatchTasks.Start();
            for (int j = 0; j < numberOfTasks; j++)
            {
                tasks[j] = Task.Factory.StartNew(() => MultiplyWithTasks(numberOfTasks, task++, matrix, matrix, lines.Length, lines.Length, lines.Length, resultTasks));
            }
            Task.WaitAll(tasks);
            stopwatchTasks.Stop();
            Console.WriteLine("Время перемножения с помощью задач: {0} mc", stopwatchTasks.ElapsedMilliseconds.ToString());

            resultParallelFor = MultiplyWithParallelFor(matrix, matrix, lines.Length, lines.Length, lines.Length);
        }

    }
}
