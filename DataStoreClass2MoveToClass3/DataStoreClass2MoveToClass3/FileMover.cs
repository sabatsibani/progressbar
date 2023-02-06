using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace DataStoreClass2MoveToClass3
{
    public static class FileMover
    {
        public static void MoveFiles(this string sourceFolderPath, string destFolderPath, Action<int> progessbarCallBack)
        {

            string parent = Directory.GetParent(destFolderPath).FullName;
            var dInfo = new DirectoryInfo(parent); // Or FileInfo
            var dSec = dInfo.GetAccessControl();

            if (!Directory.Exists(destFolderPath))
            {
                Directory.CreateDirectory(destFolderPath);
                DirectoryInfo directory = new DirectoryInfo(destFolderPath);
                directory.SetAccessControl(dSec);
            }

            // Get Files & Copy
            string[] files = Directory.GetFiles(sourceFolderPath);

            files[0] = @"F:\TestData\NA\SCGVL\PrototypeTest\OtherSysData\TR3410_se9fb\NonExpCtrl\Class2\u1\other";
            files[1] = @"F:\TestData\NA\SCGVL\PrototypeTest\OtherSysData\TR3410_se9fb\NonExpCtrl\Class2\u\other";
            files[2] = @"F:\TestData\NA\SCGVL\PrototypeTest\OtherSysData\TR3410_se9fb\NonExpCtrl\Class2\u1\other";
            files[3] = @"F:\TestData\NA\SCGVL\PrototypeTest\OtherSysData\TR3410_se9fb\NonExpCtrl\Class2\u0\other";

            // string dest = "";

            foreach (string file in files)
            {
                try
                {
                    FileInfo fileinfo = new FileInfo(file);
                    FileInfo destination = new FileInfo(destFolderPath);
                    const int bufferSize = 1024 * 1024;
                    byte[] buffer = new byte[bufferSize], buffer2 = new byte[bufferSize];
                    bool swap = false;
                    int progress = 0, reportedProgress = 0, read = 0;
                    long len = file.Length;
                    float flen = len;
                    Task writer = null;
                    using (var source = fileinfo.OpenRead())
                    using (var dest = destination.OpenWrite())
                    {
                        dest.SetLength(source.Length);
                        for (long size = 0; size < len; size += read)
                        {
                            if ((progress = ((int)((size / flen) * 100))) != reportedProgress)
                                progessbarCallBack(reportedProgress = progress);
                            read = source.Read(swap ? buffer : buffer2, 0, bufferSize);
                            writer?.Wait();
                            writer = dest.WriteAsync(swap ? buffer : buffer2, 0, read);
                            swap = !swap;
                            Thread.Sleep(1000);
                        }
                        writer?.Wait();
                        
                        //Logger.Log(string.Format("Moved File {0} to File {1}", file, dest));
                    }
                }

                catch (Exception ex)
                {
                    //Logger.Log(string.Format("Error {0} while moving File {1} to File {2}", ex.Message, file, destFolderPath), 2);
                }
            }
        }
    }
}

