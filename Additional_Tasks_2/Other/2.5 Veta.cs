using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using System.Globalization;
namespace task5
{
    class Program
    {
        static double[,] inputMatr;
        static string readfrom = @"C:\Users\Виолетта\Desktop\МИСИС\БПМ\Параллельные алгоритмы\Доп задачи 2\in.csv";
       
        //возвращает матрицу без row-ой строки и col-го столбца
        static void getMatrixWithoutRowAndCol(double [,] matrix, int size, int row, int col, double [,] newMatrix)
        {
            int changeRow = 0;
            int changeCol = 0;//смещение индекса столбца в матрице
            for (int i = 0; i < size - 1; i++)
            {
                if (i == row)
                {
                    changeRow = 1;
                }
                changeCol = 0;
                for (int j = 0; j < size - 1; j++)
                {
                    if (j == col)
                    {
                        changeCol = 1;
                    }
                    newMatrix[i, j] = matrix[i + changeRow, j + changeCol];

                }
            } 
           
        }

        static double matrixDet(double[,] matrix,int size)
        {
            double det = 0;
            int degree = 1;

            if (size != 0)
            {
                double[,] newMatrix = new double[size - 1, size - 1];
                for (int j = 0; j < size; j++)
                {
                    getMatrixWithoutRowAndCol(matrix, size, 0, j, newMatrix);
                    det = det + (degree * matrix[0, j] * matrixDet(newMatrix, size - 1));
                    degree = -degree;
                    Console.WriteLine(det);
                }
            }
           

                //foreach (double value in newMatrix)
                //{
                //    Parallel.For(0, size - 1, (i) =>
                //    {
                //        Parallel.For(0, size - 1, (j) =>
                //        {
                //            newMatrix[i, j] = Convert.ToUInt16(newMatrix[i, j]);
                //        });
                //    });
                //}
                //Parallel.For(0, size, (j) =>
                //{
                //    getMatrixWithoutRowAndCol(matrix, size, 0, j, newMatrix);
                //    det = det + (degree * matrix[0, j] * matrixDet(newMatrix, size - 1));
                //    degree = -degree;
                //});

                
            

            GC.Collect();
            //GC.WaitForPendingFinalizers();
            return det;
        }

        static void Main(string[] args)
        {
            Stopwatch watch = new Stopwatch();
            watch.Start();
            string[] input = File.ReadAllLines(readfrom);

            int matLength = input.Length;//размер матрицы

            inputMatr = new double[matLength, matLength];

            int i = 0;
            foreach (string line in input)
            {
                string[] line_s = line.Split(',');
                int j = 0;
                foreach (string str in line_s)
                {
                    inputMatr[i, j] = Double.Parse(str, CultureInfo.InvariantCulture);
                    j++;
                }
                i++;
            }
            Console.WriteLine("Матрица заполнена");

           
            double determinant = matrixDet(inputMatr, matLength);
            Console.WriteLine(determinant);
            watch.Stop();
            Console.ReadKey();
        }
    }
}
