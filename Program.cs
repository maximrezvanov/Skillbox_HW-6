using System;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace HW6
{
    class Program
    {
        static string file = "groups.txt";
        const string path = "out.txt";
        static int n = ReadFile(path);

        static void Main(string[] args)
        {
            Console.WriteLine($"\nЗагруженное число n = {n}");
            Console.WriteLine($"\nВыберите действие");
            Console.WriteLine($"1 - Показать кол-во групп");
            Console.WriteLine($"2 - Запись группы");

            int mode = int.Parse(Console.ReadLine());

            UserBehavior(mode, n);
        }

        static void UserBehavior(int mode, int n)
        {
            if (n >= 1 && n <= 1_000_000_000)
            {
                try
                {
                    switch (mode)
                    {
                        case 1:
                            var tStart = TimerStart();
                            Console.WriteLine($"\nКоличество групп: {GroupsNum(n)}");

                            var tStop = TimerStop(tStart);
                            Console.WriteLine($"\nЗатраченное время на выполение {tStop.TotalSeconds} сек");
                            Console.Read();

                            break;

                        case 2:

                            tStart = TimerStart();

                            MakeAndWriteGroups(n);

                            tStop = TimerStop(tStart);
                            Console.WriteLine($"\n\nЗатраченное время на выполение {tStop.TotalSeconds} сек");

                            Console.WriteLine($"\nЗаархивировать данные? (1 - да, 0 - нет)");
                            int number;
                            bool success = int.TryParse(Console.ReadLine(), out number);
                            if (success)
                            {
                                if (number == 1)
                                {
                                    Archive(file);
                                }
                                else
                                {
                                    break;
                                }
                            }

                            Console.Read();
                            break;

                        default:
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Ошибка: " + e);
                }
            }
        }

        static int GroupsNum(int n)
        {
            int groups = 1;
            int div = n;

            do
            {
                div /= 2;
                groups++;
            } while (div > 1);

            return groups;
        }

        static void MakeAndWriteGroups(int n)
        {

            using (StreamWriter swrite = new StreamWriter(file))
            {
                for (int i = 1; i <= GroupsNum(n); i++)
                {
                    swrite.Write($"{i}-я группа: [{String.Join(", ", NextGroup(i, GroupsNum(n), n))}] \n");
                }
            }
        }

        static int[] NextGroup(int groupNumber, int groups, int number)
        {
            if (groupNumber != groups)
            {
                return Enumerable.Range((int)Math.Pow(2, groupNumber - 1), (int)Math.Pow(2, groupNumber) - (int)Math.Pow(2, groupNumber - 1)).ToArray();
            }
            else
            {
                return Enumerable.Range((int)Math.Pow(2, groupNumber - 1), number - (int)Math.Pow(2, groupNumber - 1) + 1).ToArray();
            }
        }

        #region timer

        static DateTime TimerStart()
        {
            DateTime timenow = DateTime.Now;
            return timenow;
        }

        static TimeSpan TimerStop(DateTime starttime)
        {
            TimeSpan stopwatch = DateTime.Now.Subtract(starttime);
            return stopwatch;
        }
        #endregion

        static int ReadFile(string path)
        {
            int number;
            int result = 0;

            if (File.Exists(path))
            {
                var n = File.ReadAllText(path);
                bool success = int.TryParse(n, out number);
                if (success)
                {
                    result = number;
                }
                else
                {
                    Console.WriteLine("ошибка в N");
                }
            }
                return result;
        }

        static void Archive(string sourceFile)
        {
            string archiveFile = "groups.zip";

            using (FileStream rf = new FileStream(sourceFile, FileMode.OpenOrCreate))
            {
                using (FileStream wf = File.Create(archiveFile))
                {
                    using (GZipStream af = new GZipStream(wf, CompressionMode.Compress))
                    {
                        rf.CopyTo(af);
                    }
                }
            }

            File.Delete(sourceFile);
            Console.WriteLine($"Файл заархивирован");
        }
    }
}