using System;
using System.IO;
using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace Utilities
{
    public class ZipunzipHelper
    {

        public static void ZipFolder(string outPathname, string password, string folderName)
        {

            var fsOut = File.Create(outPathname);
            var zipStream = new ZipOutputStream(fsOut);

            zipStream.SetLevel(3); //0-9, 9 being the highest level of compression

            zipStream.Password = password;  // optional. Null is the same as not setting. Required if using AES.

            // This setting will strip the leading part of the folder path in the entries, to
            // make the entries relative to the starting folder.
            // To include the full path for each entry up to the drive root, assign folderOffset = 0.
            int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1);

            CompressFolder(folderName, zipStream, folderOffset);

            zipStream.IsStreamOwner = true; // Makes the Close also Close the underlying stream
            zipStream.Close();
        }

        // Recurses down the folder structure
        //
        private static void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset)
        {

            string[] files = Directory.GetFiles(path);

            foreach (string filename in files)
            {

                var fi = new FileInfo(filename);

                string entryName = filename.Substring(folderOffset); // Makes the name in zip based on the folder
                entryName = ZipEntry.CleanName(entryName); // Removes drive from name and fixes slash direction
                var newEntry = new ZipEntry(entryName);
                newEntry.DateTime = fi.LastWriteTime; // Note the zip format stores 2 second granularity

                // Specifying the AESKeySize triggers AES encryption. Allowable values are 0 (off), 128 or 256.
                // A password on the ZipOutputStream is required if using AES.
                //   newEntry.AESKeySize = 256;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                //   zipStream.UseZip64 = UseZip64.Off;
                newEntry.Size = fi.Length;

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                var buffer = new byte[4096];
                using (FileStream streamReader = File.OpenRead(filename))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = Directory.GetDirectories(path);
            foreach (string folder in folders)
            {
                CompressFolder(folder, zipStream, folderOffset);
            }
        }



        /// <summary>
        /// Zip a file
        /// </summary>
        /// <param name="srcFile">source file path</param>
        /// <param name="dstFile">zipped file path</param>
        /// <param name="password"></param>
        public static void Zip(string srcFile, string dstFile, string password)
        {
            var fileStreamIn = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            var fileStreamOut = new FileStream(dstFile, FileMode.Create, FileAccess.Write);
            var zipOutStream = new ZipOutputStream(fileStreamOut);
            if (!string.IsNullOrEmpty(password)) 
                zipOutStream.Password = password;
            var buffer = new byte[4096];

            var entry = new ZipEntry(Path.GetFileName(srcFile));
            zipOutStream.PutNextEntry(entry);

            int size;
            do
            {
                size = fileStreamIn.Read(buffer, 0, buffer.Length);
                zipOutStream.Write(buffer, 0, size);
            } while (size > 0);

            zipOutStream.Close();
            fileStreamOut.Close();
            fileStreamIn.Close();
        }

        /// <summary>
        /// UnZip a file
        /// </summary>
        /// <param name="srcFile">source file path</param>
        /// <param name="dstFile">unzipped file path</param>
        /// <param name="password"></param>
        public static void UnZip(string srcFile, string dstFile , string password)
        {
            var fileStreamIn = new FileStream(srcFile, FileMode.Open, FileAccess.Read);
            var zipInStream = new ZipInputStream(fileStreamIn);
            if (!string.IsNullOrEmpty(password))
                zipInStream.Password = password;
            var entry = zipInStream.GetNextEntry();
            var fileStreamOut = new FileStream(dstFile + @"\" + entry.Name, FileMode.Create, FileAccess.Write);

            int size;
            var buffer = new byte[4096];
            do
            {
                size = zipInStream.Read(buffer, 0, buffer.Length);
                fileStreamOut.Write(buffer, 0, size);
            } while (size > 0);

            zipInStream.Close();
            fileStreamOut.Close();
            fileStreamIn.Close();
        }

        public static void ExtractZipFile(string archiveFilenameIn, string password, string outFolder)
        {
            ZipFile zf = null;
            try
            {
                FileStream fs = File.OpenRead(archiveFilenameIn);
                zf = new ZipFile(fs);
                if (!String.IsNullOrEmpty(password))
                {
                    zf.Password = password;     // AES encrypted entries are handled automatically
                }
                foreach (ZipEntry zipEntry in zf)
                {
                    if (!zipEntry.IsFile)
                    {
                        continue;           // Ignore directories
                    }
                    String entryFileName = zipEntry.Name;
                    // to remove the folder from the entry:- entryFileName = Path.GetFileName(entryFileName);
                    // Optionally match entrynames against a selection list here to skip as desired.
                    // The unpacked length is available in the zipEntry.Size property.

                    var buffer = new byte[4096];     // 4K is optimum
                    Stream zipStream = zf.GetInputStream(zipEntry);

                    // Manipulate the output filename here as desired.
                    String fullZipToPath = Path.Combine(outFolder, entryFileName);
                    string directoryName = Path.GetDirectoryName(fullZipToPath);
                    if (directoryName.Length > 0)
                        Directory.CreateDirectory(directoryName);

                    // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                    // of the file, but does not waste memory.
                    // The "using" will close the stream even if an exception occurs.
                    using (FileStream streamWriter = File.Create(fullZipToPath))
                    {
                        StreamUtils.Copy(zipStream, streamWriter, buffer);
                    }
                }
            }
            finally
            {
                if (zf != null)
                {
                    zf.IsStreamOwner = true; // Makes close also shut the underlying stream
                    zf.Close(); // Ensure we release resources
                }
            }
        }

        /// <summary>
        /// Creates a zip file
        /// </summary>
        /// <param name="zipFileStoragePath">where to store the zip file</param>
        /// <param name="zipFileName">the zip file filename</param>
        /// <param name="fileToZip">the file to zip</param>
        /// <returns>indicates whether the file was created successfully</returns>
        private bool CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , FileInfo fileToZip)
        {
            return CreateZipFile(zipFileStoragePath
                                , zipFileName
                                , (FileSystemInfo)fileToZip);
        }

        /// <summary>
        /// Creates a zip file
        /// </summary>
        /// <param name="zipFileStoragePath">where to store the zip file</param>
        /// <param name="zipFileName">the zip file filename</param>
        /// <param name="directoryToZip">the directory to zip</param>
        /// <returns>indicates whether the file was created successfully</returns>
        private bool CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , DirectoryInfo directoryToZip)
        {
            return CreateZipFile(zipFileStoragePath
                                , zipFileName
                                , (FileSystemInfo)directoryToZip);
        }

        /// <summary>
        /// Creates a zip file
        /// </summary>
        /// <param name="zipFileStoragePath">where to store the zip file</param>
        /// <param name="zipFileName">the zip file filename</param>
        /// <param name="fileSystemInfoToZip">the directory/file to zip</param>
        /// <returns>indicates whether the file was created successfully</returns>
        private bool CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , FileSystemInfo fileSystemInfoToZip)
        {
            return CreateZipFile(zipFileStoragePath
                                , zipFileName
                                , new FileSystemInfo[] 
                                    { 
                                        fileSystemInfoToZip 
                                    });
        }


        /// <summary>
        /// A function that creates a zip file
        /// </summary>
        /// <param name="zipFileStoragePath">location where the file should be created</param>
        /// <param name="zipFileName">the filename of the zip file</param>
        /// <param name="fileSystemInfosToZip">an array of filesysteminfos that needs to be added to the file</param>
        /// <returns>a bool value that indicates whether the file was created</returns>
        private bool CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , FileSystemInfo[] fileSystemInfosToZip)
        {
            // a bool variable that says whether or not the file was created
            bool isCreated = false;

            try
            {
                //create our zip file
                ZipFile z = ZipFile.Create(zipFileStoragePath + zipFileName);
                //initialize the file so that it can accept updates
                z.BeginUpdate();
                //get all the files and directory to zip
                GetFilesToZip(fileSystemInfosToZip, z);
                //commit the update once we are done
                z.CommitUpdate();
                //close the file
                z.Close();
                //success!
                isCreated = true;
            }
            catch (Exception ex)
            {
                //failed
                isCreated = false;
                //lets throw our error
                throw ex;
            }

            //return the creation status
            return isCreated;
        }

        /// <summary>
        /// Iterate thru all the filesysteminfo objects and add it to our zip file
        /// </summary>
        /// <param name="fileSystemInfosToZip">a collection of files/directores</param>
        /// <param name="z">our existing ZipFile object</param>
        private void GetFilesToZip(FileSystemInfo[] fileSystemInfosToZip, ZipFile z)
        {
            //check whether the objects are null
            if (fileSystemInfosToZip != null && z != null)
            {
                //iterate thru all the filesystem info objects
                foreach (FileSystemInfo fi in fileSystemInfosToZip)
                {
                    //check if it is a directory
                    if (fi is DirectoryInfo)
                    {
                        DirectoryInfo di = (DirectoryInfo)fi;
                        //add the directory
                        z.AddDirectory(di.FullName);
                        //drill thru the directory to get all
                        //the files and folders inside it.
                        GetFilesToZip(di.GetFileSystemInfos(), z);
                    }
                    else
                    {
                        //add it
                        z.Add(fi.FullName);
                    }
                }
            }
        }

        /// <summary>
        /// A method that creates a zip file
        /// </summary>
        /// <param name="zipFileStoragePath">the storage location</param>
        /// <param name="zipFileName">the zip file name</param>
        /// <param name="fileToCompress">the file to compress</param>
        private void CreateZipFile(string zipFileStoragePath
            , string zipFileName
            , string fileToCompress)
        {
            //create our zip file
            ZipFile z = ZipFile.Create(zipFileStoragePath + zipFileName);

            //initialize the file so that it can accept updates
            z.BeginUpdate();

            //add the file to the zip file
            z.Add(fileToCompress);

            //commit the update once we are done
            z.CommitUpdate();
            //close the file
            z.Close();
        }
    }
}
