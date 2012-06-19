using System;
using System.IO;
using System.Threading;
using IISExpressAutomation;

namespace SiteMonitor.Core.Reporting
{
    public class RunResultsReporting
    {
        private static IISExpress _iis;

        public static void Initialize()
        {
            var path = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory);

            using (_iis = new IISExpress(new Parameters
                    {
                        Port = 8585,
                        Path = path
                    }
                ))
            {
                Thread.Sleep(60000);
            }
        }
    }
}
