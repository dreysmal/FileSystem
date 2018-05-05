using System;
using System.Text.RegularExpressions;
using System.IO;
using System.Collections.Generic;
using static System.Console;

namespace File_System_2
{
    class File_manage
    {
        static int i;
        public static Dictionary<int, string> Coincidences = new Dictionary<int, string>();
        public Regex regMask;
        public void GetMask()
        {
            WriteLine("Enter name of file or directory for searching or mask:");
            string mask = ReadLine();
            Clear();
            mask = mask.Replace(".", "\\.");
            mask = mask.Replace("?", ".");
            mask = mask.Replace("*", ".*");
            mask = "^" + mask + "$";
            regMask = new Regex(mask, RegexOptions.IgnoreCase);
        }
        public void Search()
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            foreach (DriveInfo drive in drives)
            {
                try
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(drive.ToString());  //drive.ToString()
            Searching(directoryInfo, regMask);
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }
        static void Searching(DirectoryInfo dir_or_file_name, Regex mask)
        {
            FileInfo[] fi = dir_or_file_name.GetFiles();
            foreach (FileInfo item in fi)
            {
                if (mask.IsMatch(item.Name))
                {
                    WriteLine((i + 1) + " File: " + item.FullName);
                    Coincidences.Add(i++, item.FullName);
                }
            }
            DirectoryInfo[] subdir = dir_or_file_name.GetDirectories();
            foreach (DirectoryInfo item in subdir)
            {
                try
                {
                    if (mask.IsMatch(item.Name))
                    {
                        WriteLine((i + 1) + " Dir: " + item.FullName);
                        Coincidences.Add(i++, item.FullName);
                    }
                    Searching(item, mask);
                }
                catch (Exception)
                {
                    //Console.WriteLine("Access denied to " + item.ToString());
                    continue;
                }
            }
        }
        public void DeleteAll()
        {
            foreach (String item in Coincidences.Values)
            {
                FileInfo fileInf = new FileInfo(item);
                if (fileInf.Attributes.HasFlag(FileAttributes.Directory))
                {
                    try
                    {
                        Directory.Delete(item, true);
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message);
                        continue;
                    }
                }
                else
                {
                    try
                    {
                        File.Delete(item);
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message);
                        continue;
                    }
                }
            }
        }
        public void DeleteSpecific(int x)
        {
            FileInfo fileInf = new FileInfo(Coincidences[x - 1]);
            if (fileInf.Attributes.HasFlag(FileAttributes.Directory))
            {
                try
                {
                    Directory.Delete(Coincidences[x - 1], true);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
            else
            {
                try
                {
                    File.Delete(Coincidences[x - 1]);
                }
                catch (Exception ex)
                {
                    WriteLine(ex.Message);
                }
            }
        }
        public void DeleteRange(int x1, int x2)
        {
            for (int i = x1-1; i < x2-1; i++)
            {
                FileInfo fileInf = new FileInfo(Coincidences[i]);
                if (fileInf.Attributes.HasFlag(FileAttributes.Directory))
                {
                    try
                    {
                        Directory.Delete(Coincidences[i], true);
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message);
                    }
                }
                else
                {
                    try
                    {
                        File.Delete(Coincidences[i]);
                    }
                    catch (Exception ex)
                    {
                        WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}
