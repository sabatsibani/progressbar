using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace DataStoreClass2MoveToClass3
{
    class DataStoreFileMover : IDataStoreFileMover
    {
        public ILogger Logger { get; set; }
        public string SourcePathName { get; set; }
        public string DestinationPathName { get; set; }

        public DataStoreFileMover(ILogger logger, string sourcePath, string destPath)
        {
            this.Logger = logger;
            this.SourcePathName = sourcePath;
            this.DestinationPathName = destPath;
        }


        public List<string> GetSourceFolderList(string sourcePath)
        {
            List<string> dirsWithdata = new List<string>();
            DirectoryInfo directory = new DirectoryInfo(sourcePath);

            DirectoryInfo[] subDirectories = directory.GetDirectories(SourcePathName, SearchOption.AllDirectories);

            foreach(DirectoryInfo dir in subDirectories)
            {
                if(dir.GetFiles("*",SearchOption.AllDirectories).ToList().Count > 0)
                {
                    Logger.Log($"Found data for directory: {dir.FullName}");
                    dirsWithdata.Add(dir.FullName);
                }
            }

            return dirsWithdata;
        }

        public bool MoveFiles(string sourceFolder, string destFolder)
        {
            try
            {
                string parent = Directory.GetParent(destFolder).FullName;
                var dInfo = new DirectoryInfo(parent); // Or FileInfo
                var dSec = dInfo.GetAccessControl();

                if (!Directory.Exists(destFolder))
                {
                    Directory.CreateDirectory(destFolder);
                    DirectoryInfo directory = new DirectoryInfo(destFolder);
                    directory.SetAccessControl(dSec);
                }

                // Get Files & Copy
                string[] files = Directory.GetFiles(sourceFolder);

                string dest = "";

                foreach (string file in files)
                {
                    try
                    {
                        string name = Path.GetFileName(file);

                        // ADD Unique File Name Check to Below!!!!
                        dest = Path.Combine(destFolder, name);

                        if (File.Exists(dest))
                        {
                            name = "Class2_" + name;
                            dest = Path.Combine(destFolder, name);
                        }

                        Logger.Log(String.Format("Moving File {0} to File {1}", file, dest));

                        File.Copy(file, dest);
                        File.Delete(file);

                        Logger.Log(String.Format("Moved File {0} to File {1}", file, dest));
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(String.Format("Error {0} while moving File {1} to File {2}", ex.Message, file, dest), 2);
                    }
                }

                // Get dirs recursively and copy files
                string[] folders = Directory.GetDirectories(sourceFolder);
                foreach (string folder in folders)
                {
                    string name = Path.GetFileName(folder);
                    dest = Path.Combine(destFolder, name);
                    try
                    {
                        Logger.Log(String.Format("Moving Folder {0} to Folder {1}", folder, dest));
                        MoveFiles(folder, dest);
                        Logger.Log(String.Format("Moved Folder {0} to File {1}", folder, dest));
                    }
                    catch (Exception ex)
                    {
                        Logger.Log(String.Format("Error {0} while moving Folder {1} to File {2}", ex.Message, folder, dest), 2);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(String.Format("Error {0} ", ex.Message), 2);
            }
            return true;
        }
    }
}
