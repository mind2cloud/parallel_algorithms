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
        static double[] resultedMatrix;
        static double[,] matrix;
        static string readFrom = @"in.csv";
        static string writeTo = @"out.csv";

        static void Main(string[] args)
        {

            // ЗАПОЛНЕНИЕ МАТРИЦЫ ИЗ ФАЙЛА
            string[] inputMatrix = File.ReadAllLines(readFrom);

            int matrixLength = inputMatrix.Length;

            matrix = new double[matrixLength, matrixLength];
            resultedMatrix = new double[matrixLength];

            int i = 0;
            foreach (string line in inputMatrix)
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
            //Console.WriteLine("Матрица успешно заполнена");


            // ВЫБОР ДЕЙСТВИЯ НАД МАТРИЦЕЙ
            Console.WriteLine("Введите: 1 - вычисление суммы, 2 - вычисление среднего");
            int choice = 0;
            choice = Convert.ToInt32(Console.ReadLine());

            switch (choice)
            {
                case(1):
                    double resultFirst = 0;
                    for (int k = 0; k < matrixLength; k++)
                    {
                        Parallel.For(0, matrixLength, (j) => {
                            resultFirst += matrix[k, j];
                        });
                        resultedMatrix[k] = resultFirst;
                        resultFirst = 0;
                    }
                    break;

                case(2):
                    double resultSecond = 0;
                    for (int k = 0; k < matrixLength; k++)
                    {
                        Parallel.For(0, matrixLength, (j) => {
                            resultSecond += matrix[k, j];
                        });
                        resultedMatrix[k] = resultSecond / matrixLength;
                        resultSecond = 0;
                    }
                    break;

                default:
                    break;
            }

            Console.WriteLine("Вычисление прошло успешно");


            // ЗАПИСЬ РЕЗУЛЬТАТА ОПЕРАЦИИ В ФАЙЛ
            string[] result = new string[matrixLength];

            for (int w = 0; w < matrixLength; w++)
            {
                result[w] = resultedMatrix[w].ToString();
            }

            File.WriteAllLines(writeTo, result);
            Console.WriteLine("Успешно записано в файл");
        }
    }

}
