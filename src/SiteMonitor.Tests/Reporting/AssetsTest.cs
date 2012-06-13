using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SiteMonitor.Reporting;
using System.Threading;

namespace SiteMonitor.Tests.Reporting
{
    [TestFixture]
    public class AssetsTest
    {
        [Test]
        public void can_get_assets()
        {
            RunResultsReporting.Initialize();            
        }
    }
}
