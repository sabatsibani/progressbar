using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataStoreClass2MoveToClass3
{
    class Logger : ILogger
    {
        public void Log(string message, int LogType = 1 /*Info*/)
        {
            string fileName = string.Format("{0}_log.csv", DateTime.Today.ToString("ddMMyyyy"));

            if (!File.Exists(fileName))
            {
                File.Create(fileName).Close();
            }

            if(LogType == 2)
            {
                message = string.Format("{0},{1},{2} ",DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"),"Error",message);
            }
            else
            {
                message = string.Format("{0},{1},{2} ", DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss"), "Info", message);
            }

            using (StreamWriter w = File.AppendText(fileName))
            {
                w.WriteLine(message);
            }

        }
    }
}
