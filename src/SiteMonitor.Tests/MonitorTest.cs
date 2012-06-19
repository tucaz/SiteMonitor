using SiteMonitor.Tests.SampleRunners;

namespace SiteMonitor.Tests
{
    public static class MonitorTest
    {
        public static void Main()
        {
            Monitor.AddRunner(new RandomTimeRunner());
            for (int i = 0; i < 100; i++)
            {
                Monitor.StartMonitoring();
            }            
        }
    }
}
