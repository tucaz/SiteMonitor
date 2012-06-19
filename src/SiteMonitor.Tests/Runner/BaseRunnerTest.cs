using NUnit.Framework;
using SharpTestsEx;
using SiteMonitor.Runner;
using SiteMonitor.Tests.SampleRunners;

namespace SiteMonitor.Tests.Runner
{
    [TestFixture]
    public class BaseRunnerTest
    {
        [Test]
        public void will_record_run_time()
        {
            BaseRunner runner = new RandomTimeRunner();
            runner.Run();

            runner.TimeTaken.TotalMilliseconds.Should().Be.GreaterThan(0);
        }

        [Test]
        public void will_return_default_name_same_as_class_name()
        {
            BaseRunner runner = new RandomTimeRunner();
            
            runner.RunnerName.Should().Be.EqualTo(runner.GetType().Name);
        }

        [Test]
        public void will_restart_time_counter_in_every_run()
        {
            BaseRunner runner = new FixedTimeRunner(100);   
            
            runner.Run();
            runner.Run();

            runner.TimeTaken.TotalMilliseconds.Should().Be.LessThan(101);
        }
    }   
}
