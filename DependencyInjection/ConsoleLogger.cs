using System;
using System.Collections.Generic;
using System.Text;

namespace DependencyInjection
{
    class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            string messageWithTime = $"({DateTime.Now:HH:mm:ss}) {message}";
            Console.WriteLine(messageWithTime);
        }
    }
}
