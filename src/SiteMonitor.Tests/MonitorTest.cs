using NUnit.Framework;
using SharpTestsEx;
using SiteMonitor.Core;
using SiteMonitor.Tests.SampleRunners;

namespace SiteMonitor.Tests
{
    [TestFixture]
    public class MonitorTest
    {        
        private Monitor _monitor;
        
        [SetUp]
        public void Setup()
        {
            _monitor = new Monitor();
        }
        
        [Test]
        public void can_add_runner() 
        {
            _monitor.AddRunner(new RandomTimeRunner());

            _monitor.RunnersLoaded.Count.Should().Be.EqualTo(1);
        }

        [Test]
        public void can_load_runners_from_assembly()
        {
            string assemblyLocation = "SiteMonitor.Tests.dll";
            _monitor.AddRunnersFromAssembly(assemblyLocation);

            _monitor.RunnersLoaded.Count.Should().Be.EqualTo(3);
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
