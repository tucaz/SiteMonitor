using NUnit.Framework;
using SharpTestsEx;
using SiteMonitor.Core;
using SiteMonitor.Tests.SampleRunners;

namespace SiteMonitor.Tests
{
    [TestFixture]
    public class MonitorTest
    {        
        [Test]
        public void can_add_runner() 
        {
            Monitor.AddRunner(new RandomTimeRunner());

            Monitor.RunnersLoaded.Count.Should().Be.EqualTo(1);
        }

        [Test]
        public void can_load_runners_from_assembly()
        {
            string assemblyLocation = "SiteMonitor.Tests.dll";
            Monitor.AddRunnersFromAssembly(assemblyLocation);

            Monitor.RunnersLoaded.Count.Should().Be.EqualTo(3);
        }

        //public static void Main()
        //{
        //    Monitor.AddRunner(new RandomTimeRunner());
        //    Monitor.AddRunner(new FixedTimeRunner(500));
        //    Monitor.StartMonitoring(1);            

        //    T.Thread.Sleep(6000000);
        //}
    }
}
