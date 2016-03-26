using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ionic.Zip;

namespace Utilities
{
    public static class Zipper
    {
        public const string ZIPEXTENTION =".zip";
        public static void ZIPFoder(string folderSource, string fileNameDestination)
        {
                using (ZipFile zip = new ZipFile())
                {
                    ZipEntry e = zip.AddDirectory(folderSource);
                    if (!fileNameDestination.ToLower().EndsWith(ZIPEXTENTION))
                        fileNameDestination = string.Concat(fileNameDestination, ZIPEXTENTION);
                    zip.Save(fileNameDestination);
                }
                
        }
    }
}
