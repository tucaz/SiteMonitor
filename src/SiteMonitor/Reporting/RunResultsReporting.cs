using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using IISExpressAutomation;
using System.IO;

namespace SiteMonitor.Reporting
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
