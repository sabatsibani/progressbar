using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreClass2MoveToClass3
{
    interface IDataStoreFileMover
    {
        List<string> GetSourceFolderList(string sourcePath);

        bool MoveFiles(string sourcePath, string destinationPath);
    }
}
