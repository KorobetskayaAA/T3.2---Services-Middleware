using System;
using System.Threading.Tasks;

namespace DependencyInjection
{
    class Program
    {
        static void Main(string[] args)
        {
            ILogger logger = new FileLogger();
            
            new CpuMonitor(logger).Run(2000);
            new RamMonitor(logger).Run(5000);

            var inputController = new InputController(logger);

            while (!string.IsNullOrEmpty(inputController.Input()));
        }
    }
}
