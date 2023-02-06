using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreClass2MoveToClass3
{
    interface ILogger
    {
        void Log(string message, int LogType = 1 /*Info*/);
    }
}
