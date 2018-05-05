//2. Написать приложение для поиска по всему диску файлов и
//каталогов, удовлетворяющих заданной маске. Необходимо вывести
//найденную информацию на экран в компактном виде (с нумерацией
//объектов) и запросить у пользователя о дальнейших действиях.
//Варианты действий: удалить все найденное, удалить указанный файл
//(каталог), удалить диапазон файлов(каталогов). 
using static System.Console;
using System;

namespace File_System_2
{
    class Program
    {
        static void Main()
        {
            File_manage F_manager = new File_manage();
            F_manager.GetMask();
            F_manager.Search();
            if(File_manage.Coincidences.Count>0)
            {
                WriteLine("\n" + @"Press:
1 - Delete all found files and directories.
2 - Enter menu of deleting chosen file or directory.
3 - Enter menu of entering range of numbers of files and directories to delete.");
                bool flag = true;
                while (flag)
                {
                    ConsoleKeyInfo keypress = ReadKey();
                    switch (keypress.Key)
                    {
                        case ConsoleKey.D1:
                        case ConsoleKey.NumPad1:
                            flag = false;
                            F_manager.DeleteAll();
                            WriteLine("All files deleted");
                            break;
                        case ConsoleKey.D2:
                        case ConsoleKey.NumPad2:
                            flag = false;
                            WriteLine("\nEnter number of file or directory to delete.");
                            int x = 0;
                            while (true)
                            {
                                try
                                {
                                    x = Int32.Parse(ReadLine());
                                    break;
                                }
                                catch (FormatException ex)
                                {
                                    WriteLine(ex.Message + "try again");
                                    continue;
                                }
                                catch (ArgumentNullException ex1)
                                {
                                    WriteLine(ex1.Message + "try again");
                                    continue;
                                }
                            }
                            F_manager.DeleteSpecific(x);
                            break;
                        case ConsoleKey.D3:
                        case ConsoleKey.NumPad3:
                            flag = false;
                            WriteLine("\nSet Range of files or directories to delete.\nEnter first number:");
                            int x1 = 0;
                            int x2 = 0;
                            while (true)
                            {
                                try
                                {
                                    x1 = Int32.Parse(ReadLine());
                                    break;
                                }
                                catch (FormatException ex)
                                {
                                    WriteLine(ex.Message + "try again");
                                    continue;
                                }
                                catch (ArgumentNullException ex1)
                                {
                                    WriteLine(ex1.Message + "try again");
                                    continue;
                                }
                            }
                            WriteLine("Enter second number:");
                            while (true)
                            {
                                try
                                {
                                    x2 = Int32.Parse(ReadLine());
                                    break;
                                }
                                catch (FormatException ex)
                                {
                                    WriteLine(ex.Message + "try again");
                                    continue;
                                }
                                catch (ArgumentNullException ex1)
                                {
                                    WriteLine(ex1.Message + "try again");
                                    continue;
                                }
                            }
                            F_manager.DeleteRange(x1, x2);
                            break;
                        default:
                            flag = true;
                            break;
                    }
                }
            }
        }
    }
}
