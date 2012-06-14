
using SiteMonitor.DB;
using System;
namespace SiteMonitor.Tests
{
    public static class MonitorTest
    {
        public static void Main()
        {
            Monitor.AddRunner(new GoogleSearchRunner());

            Monitor.StartMonitoring();

            var db = new Database();
            db.GetRunResults("GoogleSearchRunner", DateTime.Now.AddDays(-1), DateTime.Now.AddDays(1));
        }
    }
}
