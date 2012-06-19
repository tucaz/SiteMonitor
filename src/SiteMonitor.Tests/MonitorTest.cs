using System.Threading;
using SiteMonitor.Tests.SampleRunners;

namespace SiteMonitor.Tests
{
    public static class MonitorTest
    {
        public static void Main()
        {
            Monitor.AddRunner(new RandomTimeRunner());
            Monitor.StartMonitoring(1);            

            Thread.Sleep(6000000);
        }
    }
}
