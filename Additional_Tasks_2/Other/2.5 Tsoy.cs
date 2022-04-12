using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace extra2_5_det
{
    class Program
    {
       
        static int[,] minor(ref int[,] m, int nColumn)
        {
            int lenth = m.GetLength(0);
            int[,] minorN = new int[lenth - 1, lenth - 1];

            for (int i = 1; i < lenth; i++)
            {
                for (int j = 0, col = 0; j < lenth; j++)
                {
                    if (j == nColumn) continue;
                    minorN[i - 1, col++] = m[i, j];
                }
            }
            return minorN;
        }

        static int detemine(ref int[,] m)
        {
            int det = 0;
            if (m.Length == 4)
            {
                det = m[0, 0] * m[1, 1] - m[1, 0] * m[0, 1];
            }
            else
            {
                int sign = 1;
                for (int i = 0; i < m.GetLength(0); i++)
                {
                    int[,] minorN = minor(ref m, i);
                    det += sign * m[0, i] * detemine(ref minorN);
                    sign *= -1;
                }
            }
            return det;
        }


        static void Main(string[] args)
        {
            int[,] test =
            {
                {11, 12, 13, 14 },
                                {21, 13, 23, 24 },
                                                {31, 32, 17, 34 },
                                                                {41, 42, 43, 19 }
            };
            int x = detemine(ref test);
            Console.WriteLine(x);

            int N = 12;
            int[,] a = new int[N, N];
            Random random = new Random();
            for (int i = 0; i < N; i++)
            {
                for (int j = 0; j < N; j++)
                {
                    a[i, j] = random.Next(10);
                }
            }

            int detSynch, detAsynch = 0;
            Stopwatch sWatch = new Stopwatch();
            sWatch.Start();
            detSynch = detemine(ref a);
            sWatch.Stop();
            Console.WriteLine("determine: " + detSynch + " time: " + sWatch.ElapsedMilliseconds);

            int nTarget = 5;
            int h = a.GetLength(0) / nTarget;



            Task<int>[] tasks = new Task<int>[nTarget];

            Stopwatch sWatchP = new Stopwatch();
            sWatchP.Start();
            for (int n = 0; n < nTarget; n++)
            {
                tasks[n] = new Task<int>((num) =>
               {
                   int numT = (int)num;
                   int sign, det = 0;
                   if ((h * numT) % 2 == 0)
                   {
                       sign = 1;
                   }
                   else
                   {
                       sign = -1;
                   }

                   int end = (numT + 1) * h;
                   if (numT == nTarget - 1)
                   {
                       end = a.GetLength(0);
                   }


                   for (int j = numT * h; j < end; j++)
                   {
                       if (numT == nTarget)
                       {
                           Console.WriteLine("exeption");
                           break;
                       }
                       int[,] minorN = minor(ref a, j);
                       det += sign * a[0, j] * detemine(ref minorN);
                       sign *= -1;
                   }
                   return det;
               }, n
                );
        }

            for (int i = 0; i < nTarget; i++)
            {
                tasks[i].Start();
            }
            Task.WaitAll(tasks);
            for (int i = 0; i < nTarget; i++)
            {
                detAsynch += tasks[i].Result;
            }

            sWatchP.Stop();
            Console.WriteLine("determine: " + detAsynch + " time: " + sWatchP.ElapsedMilliseconds);

        }
    }
}
