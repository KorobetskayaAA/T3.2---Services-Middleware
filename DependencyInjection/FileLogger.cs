using System;
using System.IO;

namespace DependencyInjection
{
    class FileLogger : ILogger
    {
        readonly string fileName;

        public FileLogger(string fileName = "current.log")
        {
            this.fileName = fileName;
        }

        public void Log(string message)
        {
            string messageWithTime = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}: {message}{Environment.NewLine}";
            File.AppendAllText(fileName, messageWithTime);
        }
    }
}
