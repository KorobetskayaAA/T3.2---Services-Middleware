using System.Diagnostics;
using System.Threading.Tasks;

namespace DependencyInjection
{
    class CpuMonitor
    {
        readonly ILogger logger;

        public CpuMonitor(ILogger logger)
        {
            this.logger = logger;
        }

        PerformanceCounter cpuCounter =
            new PerformanceCounter("Processor", "% Processor Time", "_Total");

        public float GetCpuLoad()
        {
            return cpuCounter.NextValue();
        }

        public Task Run(int periodMs)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(periodMs);
                    logger.Log($"Загрузка процессора {GetCpuLoad():F2}%");
                }
            });
        }
    }
}
