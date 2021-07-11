using System;
using System.IO;
using System.IO.Compression;

namespace HW6
{
    class Program
    {


        static void Main(string[] args)
        {
            const string path = "out.txt";
            int n = ReadFile(path);

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

                            int[][] groups = MakeGroups(n);
                            string file = WriteFile(groups);

                            tStop = TimerStop(tStart);
                            Console.WriteLine($"\n\nЗатраченное время на выполение {tStop.TotalSeconds} сек");

                            Console.WriteLine($"\nЗаархивировать данные? (1 - да, 0 - нет)");
                            if (int.Parse(Console.ReadLine()) == 1)
                            {
                                Archive(file);
                            }
                            else
                            {
                                break;
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

        static int[][] MakeGroups(int n)
        {
            bool newGroup, isAdded;
            int[][] groups = new int[0][];

            for (int i = 1; i <= n; i++)
            {
                newGroup = false;

                if (i == 1 && groups.Length < i)
                {
                    Array.Resize(ref groups, groups.Length + 1);
                    groups[groups.Length - 1] = new int[0];
                    Array.Resize(ref groups[groups.Length - 1], groups[groups.Length - 1].Length + 1);
                    groups[groups.Length - 1][groups[groups.Length - 1].Length - 1] = i;
                }
                else
                {
                    for (int j = 0; j < groups.Length; j++)
                    {
                        isAdded = true;
                        for (int k = 0; k < groups[j].Length; k++)
                        {
                            if (i % groups[j][k] == 0)
                            {
                                isAdded = false;
                                break;
                            }
                        }
                        if (isAdded)
                        {
                            Array.Resize(ref groups[j], groups[j].Length + 1);
                            groups[j][groups[j].Length - 1] = i;
                            break;
                        }
                        else if (!isAdded && j == groups.Length - 1)
                        {
                            newGroup = true;
                        }
                    }

                    if (newGroup)
                    {
                        Array.Resize(ref groups, groups.Length + 1);
                        groups[groups.Length - 1] = new int[0];
                        Array.Resize(ref groups[groups.Length - 1], groups[groups.Length - 1].Length + 1);
                        groups[groups.Length - 1][groups[groups.Length - 1].Length - 1] = i;
                    }
                }
            }
            return groups;
        }

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

        static int ReadFile(string path)
        {
            var n = File.ReadAllText(path);
            return int.Parse(n);
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

        static string WriteFile(int[][] groups)
        {
            string file = "groups.txt";

            using (StreamWriter swrite = new StreamWriter(file))
            {
                for (int m = 0; m < groups.Length; m++)
                {
                    swrite.WriteLine($"Группа {m + 1}:");
                    for (int j = 0; j < groups[m].Length; j++)
                    {
                        swrite.Write($"{Convert.ToString(groups[m][j])} ");
                    }
                    swrite.WriteLine("\n");
                }
            }
            return file;
        }

    }
}