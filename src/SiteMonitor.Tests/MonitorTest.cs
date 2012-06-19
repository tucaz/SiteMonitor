using SiteMonitor.Core;
using SiteMonitor.Tests.SampleRunners;
using T = System.Threading;

namespace SiteMonitor.Tests
{
    public static class MonitorTest
    {
        public static void Main()
        {
            Monitor.AddRunner(new RandomTimeRunner());
            Monitor.AddRunner(new FixedTimeRunner(500));
            Monitor.StartMonitoring(1);            

            T.Thread.Sleep(6000000);
        }
    }
}
