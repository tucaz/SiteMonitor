using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace SiteMonitor.Runner
{
    public abstract class BaseRunner
    {
        private IWebDriver _driver;
        private Stopwatch _sw = new Stopwatch();

        public TimeSpan? TimeTaken { get; private set; }

        public virtual string RunnerName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public void Run()
        {
            Setup();

            try
            {
                _sw.Start();
                this.Run(_driver);
                _sw.Stop();

                TimeTaken = _sw.Elapsed;
            }
            finally
            {
                this.TearDown();
            }
        }

        protected abstract bool Run(IWebDriver driver);

        private void Setup()
        {
            //TODO: Make it a parameter
            _driver = new FirefoxDriver();
        }

        private void TearDown()
        {
            _driver.Quit();
        }
    }
}
