using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Utilities
{
    /// <summary>
    /// Text File IO Helper
    /// </summary>
    public static class IoHelper
    {

        public static bool CopyDirectory(string SourcePath, string DestinationPath, bool overwriteexisting)
        {
            bool ret;
            try
            {
                SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
                DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

                if (Directory.Exists(SourcePath))
                {
                    if (Directory.Exists(DestinationPath) == false)
                        Directory.CreateDirectory(DestinationPath);

                    foreach (string fls in Directory.GetFiles(SourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(DestinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(SourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, DestinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch 
            {
                ret = false;
            }
            return ret;
        }



        /// <summary>
        /// Add Line To Text File
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void AddLineToTextFile(string fileName, string data)
        {
            using (StreamWriter sw = new StreamWriter(fileName,true,Encoding.UTF8))
            {
                sw.WriteLine(string.Format("DateTime : {0}. Data {1}",DateTime.Now.ToString(),data));
            }

        }


        /// <summary>
        /// DeleteDirectoryFolders
        /// </summary>
        /// <param name="dirInfo"></param>
        public static void DeleteDirectoryFolders(DirectoryInfo dirInfo)
        {
            foreach (DirectoryInfo dirs in dirInfo.GetDirectories())
            {
                dirs.Delete(true);
            }

        }

        /// <summary>
        /// Add Line To Text File
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void DeleteFile(string fileName)
        {

            if (File.Exists(fileName)) 
                File.Delete(fileName); 

        }

        public static int DeleteFiles(string folder, string searchPattern, bool includeSubdirs)
        {
            int deleted = 0;
            DirectoryInfo di = new DirectoryInfo(folder);
            foreach (FileInfo fi in di.GetFiles(searchPattern, includeSubdirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly))
            {
                fi.Delete();
                deleted++;
            }

            return deleted;
        }
        public static void DeleteFolder(string folder, bool isRecurcive)
        {
            if (Directory.Exists(folder))
                Directory.Delete(folder, isRecurcive);

        }


        /// <summary>
        /// Copy File
        /// </summary>
        /// <param name="oldName"></param>
        /// <param name="newName"></param>
        public static void CopyFile(string oldName, string newName)
        {
            if (File.Exists(oldName))
                File.Copy(oldName,newName,true);

        }
        /// <summary>
        /// Get File Name Without Extension
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static string GetFileNameWithoutExtension(string file)
        {
            if (File.Exists(file))
                return Path.GetFileNameWithoutExtension(file);
            else
                return file;

        }


        /// <summary>
        /// Is Folder Exists
        /// </summary>
        /// <param name="folderName"></param>
        /// <returns></returns>
        public static bool IsFolderExists(string folderName)
        {
            return Directory.Exists(folderName);   
        }

        /// <summary>
        /// Is File Exists
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileExists(string fileName)
        {
            return (File.Exists(fileName));
        }


        /// <summary>
        /// Write To File
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="data"></param>
        public static void WriteToFile(string fileName, string data)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.GetEncoding("UTF-8")))
            {
                writer.Write(data);
            }
        }

        /// <summary>
        /// Read From File
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string ReadFromFile(string fileName)
        {
            if (IoHelper.IsFileExists(fileName))
            {
                using (StreamReader reader = new StreamReader(fileName, Encoding.UTF8, false))
                {
                    return reader.ReadToEnd();
                }
            }
            return string.Empty;
        }

        /// <summary>
        /// Create Folder
        /// </summary>
        /// <param name="folder"></param>
        public static void CreateFolder(string folder)
        {
            Directory.CreateDirectory(folder);
        }

        /// <summary>
        /// Create File
        /// </summary>
        /// <param name="folder"></param>
        public static void CreateFile(string fileFulPath)
        {
           using(FileStream fs = File.Create(fileFulPath))
           {
               fs.Flush();
           }
        }
        /// <summary>
        /// IsFileEmpty
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool IsFileEmpty(string fileName)
        {
            if (!File.Exists(fileName))
            {
                return true;
            }
            else
            {
                return (new FileInfo(fileName).Length == 0);
            }
        }

        /// <summary>
        /// Get File Name
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        public static string GetFileName(string fileName,string folder)
        {
            string fileNameFullPath = Path.Combine(folder, fileName);
            if (!IoHelper.IsFileExists(fileNameFullPath))
                IoHelper.CreateFile(fileNameFullPath);
            return fileNameFullPath;
        }

        /// <summary>
        /// Combine Path
        /// </summary>
        /// <param name="path1"></param>
        /// <param name="path2"></param>
        public static string  CombinePath(string path1, string path2)
        {
            return Path.Combine(path1,path2);   
        }
    }
}
