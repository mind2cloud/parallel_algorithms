using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ManagingStreams
{
    class Program
    {
        static void Main(string[] args)
        {
            // Для прерывания выполняемой задачи
            // создаем объект CancellationTokenSource
            // и получаем токен из этого объекта
            var cancellationTokenSource = new CancellationTokenSource();
            var token = cancellationTokenSource.Token;

            int numberOfThreads = 3;

            char inputCommand = ' ';
            int lastTaskNumber = 0;
            bool breakLast = false;

            Console.WriteLine("Хай, нажмите S для запуска потоков, А для прерывания последнего созданного потока: ");
            while (inputCommand != 'Q')
            {
                inputCommand = Console.ReadLine()[0];
                if (inputCommand == 'A')
                {
                    breakLast = true;
                }

                if (inputCommand == 'F')
                {
                    cancellationTokenSource.Cancel();
                }

                if (inputCommand == 'S')
                {
                    lastTaskNumber = 3;
                    for (int i = 0; i < numberOfThreads; i++)
                    {
                        Task<int> task = new Task<int>(() =>
                        {
                            for (int j = 0; j < int.MaxValue; ++j)
                            {
                                if (token.IsCancellationRequested)
                                {
                                    Console.WriteLine("Поток #{0}, досчитал до {1}.", Task.CurrentId, j);
                                    return j;
                                } else {
                                    if (breakLast == true)
                                    {
                                        if (Task.CurrentId == (lastTaskNumber))
                                        {
                                            lastTaskNumber--;
                                            breakLast = false;
                                            Console.WriteLine("Поток #{0}, прерван на  {1}.", Task.CurrentId, j);
                                            return j;
                                        }
                                    }
                                    Console.WriteLine(j);
                                    Thread.Sleep(5);
                                }
                            }


                            Console.WriteLine();
                            return int.MaxValue;
                        }, token);
                        task.Start();
                    }
                }

            }
        }

    }
}





//https://metanit.com/sharp/tutorial/12.5.php