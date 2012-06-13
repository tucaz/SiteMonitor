using NUnit.Framework;
using OpenQA.Selenium;
using SharpTestsEx;
using SiteMonitor.Runner;

namespace SiteMonitor.Tests.Runner
{
    [TestFixture]
    public class BaseRunnerTest
    {
        [Test]
        public void will_record_run_time()
        {
            BaseRunner runner = new FakeRunner();
            runner.Run();

            runner.TimeTaken.HasValue.Should().Be.True();
        }

        [Test]
        public void will_return_default_name_same_as_class_name()
        {
            BaseRunner runner = new FakeRunner();
            
            runner.RunnerName.Should().Be.EqualTo(runner.GetType().Name);
        }
    }

    internal class FakeRunner : BaseRunner
    {
        protected override bool Run(IWebDriver driver)
        {
            return true;
        }
    }
}
