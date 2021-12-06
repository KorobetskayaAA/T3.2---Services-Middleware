using System.Diagnostics;
using System.Threading.Tasks;

namespace DependencyInjection
{
    class RamMonitor
    {
        readonly ILogger logger;

        public RamMonitor(ILogger logger)
        {
            this.logger = logger;
        }

        PerformanceCounter ramCounter =
             new PerformanceCounter("Memory", "Available MBytes");

        public float GetRamLoad()
        {
            return ramCounter.NextValue();
        }

        public Task Run(int periodMs)
        {
            return Task.Run(async () =>
            {
                while (true)
                {
                    await Task.Delay(periodMs);
                    logger.Log($"Свободная память {GetRamLoad():N1} Мб");
                }
            });
        }
    }
}
