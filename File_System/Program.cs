//1. Написать приложение, которое ищет в указанном каталоге файлы,
//удовлетворяющие заданной маске, у которых дата последней
//модификации находится в указанном диапазоне.Поиск производится как
//в указанном каталоге, так и в его подкаталогах.Результаты поиска
//сбрасываются в файл отчета.

using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Text;

namespace File_System
{
    class Program
    {
        static ulong CountMatches = 0;
        static void Main(string[] args)
        {
            Console.WriteLine("Enter directory path:");
            string directory = Console.ReadLine();
            DirectoryInfo directoryInfo;
            while (true)
            {
                directoryInfo = new DirectoryInfo(directory);
                if (!directoryInfo.Exists)
                {
                    Console.WriteLine("You entered wrong path! Press any key to try again or press Esc to exit");
                    ConsoleKeyInfo keypress = Console.ReadKey();
                    if (keypress.Key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        Environment.Exit(-1);
                    }
                    Console.Clear();
                    Console.WriteLine("Enter directory path:");
                    directory = Console.ReadLine();
                }
                else
                {
                    break;
                }
            }
            Console.WriteLine("Enter name of file for searching or mask:");
            string mask = Console.ReadLine();

            Console.WriteLine("Enter Time period of last modifications of files.\nEnter first date of the range in format dd.mm.yyyy:");
            DateTime datet1 = DateCheck();
            Console.WriteLine("Enter Second date of the range in format dd.mm.yyyy:");
            DateTime datet2 = DateCheck();

            if (directory[directory.Length - 1] != '\\')
                directory += '\\';

            mask = mask.Replace(".", "\\.");
            mask = mask.Replace("?", ".");
            mask = mask.Replace("*", ".*");
            mask = "^" + mask + "$";

            Regex regMask = new Regex(mask, RegexOptions.IgnoreCase);
            try
            {
                using (FileStream fs = new FileStream("Search_Log.txt", FileMode.Create)) { }
                Searching(directoryInfo, regMask, datet1, datet2);
            }
            catch(NotImplementedException ex)
            {
                Console.WriteLine(ex.Message);
            }
            using (FileStream fs = new FileStream("Search_Log.txt", FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter strr = new StreamWriter(fs, Encoding.Unicode))
                {
                    strr.WriteLine("Number of files: " + CountMatches);
                }
            }
        }

        static DateTime DateCheck()
        {
            while (true)
            {
                string date1 = Console.ReadLine();
                DateTime dateEntered;
                if (Regex.IsMatch(date1, @"^(0[1-9]|1[0-9]|2[0-9]|3[0-1])\.(0[1-9]|1[0-2])\.[0-9][0-9][0-9][0-9]$"))
                {
                    string[] dateParts = date1.Split('.');
                    try
                    {
                        dateEntered = new DateTime(Convert.ToInt32(dateParts[2]), Convert.ToInt32(dateParts[1]), Convert.ToInt32(dateParts[0]));
                        return dateEntered;
                    }
                    catch (ArgumentOutOfRangeException ex)
                    {
                        Console.WriteLine("You entered wrong date. Try again");
                        Console.WriteLine(ex.Message);
                        continue;
                    }
                }
                else
                    Console.WriteLine("You entered wrong date. Try again");
                continue;
            }
        }

        static void Searching(DirectoryInfo directoryInfo, Regex regMask, DateTime date1, DateTime date2)
        {
            try
            {
                FileInfo[] fi = directoryInfo.GetFiles();
                foreach (FileInfo item in fi)
                {
                    if (regMask.IsMatch(item.Name) && (item.LastWriteTime >= date1 && item.LastWriteTime <= date2))
                    {
                        CountMatches++;
                        using (FileStream fs = new FileStream("Search_Log.txt", FileMode.Append, FileAccess.Write))
                        {
                            using (StreamWriter strr = new StreamWriter(fs, Encoding.Unicode))
                            {
                                strr.WriteLine(item.FullName + "   " + item.LastWriteTime);
                            }
                        }
                    }
                }
            }
            catch(IOException ex)
            {
                Console.WriteLine(ex.Message);
            }

            DirectoryInfo[] subdir = directoryInfo.GetDirectories();
                foreach (DirectoryInfo item in subdir)
                {
                    try
                    {
                        Searching(item, regMask, date1, date2);
                    }
                    catch(Exception)
                    {
                        Console.WriteLine("Access denied to " + item.ToString());
                        continue;
                    }
                }
        }
    }
}
