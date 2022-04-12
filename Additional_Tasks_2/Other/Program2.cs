using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Matrix
{
    class Program
    {
        static double[,] mResult;
        static double[,] matrix;
        static string readfrom = @"C:\Users\Виолетта\Desktop\МИСИС\БПМ\Параллельные алгоритмы\Доп задачи 2\matrix.csv";
        //static string writeto = @"C:\Users\Виолетта\Desktop\МИСИС\БПМ\Параллельные алгоритмы\Доп задачи 2\out.csv";

        static void Main(string[] args)
        {
            //filling the matrix
            Stopwatch watch = new Stopwatch();

            string[] inputMatr = File.ReadAllLines(readfrom);

            int matLength = inputMatr.Length;//размер матрицы

            matrix = new double[matLength, matLength];
            mResult = new double[matLength, matLength];

            int i = 0;
            foreach (string line in inputMatr)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    matrix[i, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    j++;
                }
                i++;
            }
            Console.WriteLine("Матрица заполнена");

            string resultss = "";
            for (int m = 0; m < 10; m++)
            {
                Console.WriteLine("Введите кол-во тасков");
                int kTasks = Convert.ToInt32(Console.ReadLine());
 
                watch.Start();
                Task<double>[] taskToCount = new Task<double>[kTasks];
                for (int ss = 0; ss < kTasks; ss++)
                {
                    int length = matLength / kTasks;
                    int left = ss * length;
                    int right = left + length;

                    taskToCount[ss] = new Task<double>(() => {
                        Task<double>[,] tasks = new Task<double>[matLength, matLength];
                        for (int k = left; k < right - 1; k++)
                        {
                            for (int j = left; j < right - 1; j++)
                            {
                                tasks[k, j] = new Task<double>(() => calc(k, j, right - 1));
                                tasks[k, j].Start();
                                //Console.WriteLine(tasks[k, j].Result);
                            }
                        }

                        watch.Stop();
                        Console.WriteLine("Вычислено успешно +" + watch.ElapsedMilliseconds);
                        return watch.ElapsedMilliseconds;
                    });

                    taskToCount[ss].Start();
                    taskToCount[ss].Wait();
                }
                resultss += taskToCount[0].Result.ToString() + " " + kTasks+ " ; ";
                watch.Reset();
            }
            File.WriteAllText(@"C:\Users\Виолетта\Desktop\МИСИС\БПМ\Параллельные алгоритмы\Доп задачи 2\WriteText.txt", resultss);
            Console.ReadKey();

        }

        static double calc(int i, int j, int L)
        {
            double res = 0;
            for (int k = 0; k < L; k++)
            {
                res += matrix[k, j] * matrix[i, k];
            }
            return res;
        }

        static string numstostr(int row, int L, Task<double>[,] m)
        {
            string res = m[row, 0].Result.ToString();
            for (int i = 1; i < L-1; i++)
            {
                res += "," + m[row, i].Result;
            }
            return res;
        }
    }

}
