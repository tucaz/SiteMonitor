using System;
using System.Diagnostics;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using SiteMonitor.Core.Log;

namespace SiteMonitor.Core.Runner
{
    public abstract class BaseRunner
    {
        private IWebDriver _driver;
        private Stopwatch _sw = new Stopwatch();
        public TimeSpan TimeTaken { get; private set; }
        
        public virtual string RunnerName
        {
            get
            {
                return this.GetType().Name;
            }
        }

        public virtual string Title
        {
            get
            {
                return String.Empty;
            }
        }

        public virtual string YAxisLegend
        {
            get
            {
                return "Time Taken (seconds)";
            }
        }

        public bool Run()
        {
            var error = true;
            
            Setup();

            try
            {                
                _sw.Reset();
                _sw.Start();
                error = this.Run(_driver);
                _sw.Stop();

                this.TimeTaken = _sw.Elapsed;
            }
            finally
            {
                this.TearDown();
            }

            return error;
        }

        protected abstract bool Run(IWebDriver driver);

        protected void Log(string message)
        {
            message = String.Concat(this.RunnerName, " -> ", message);
            message.LogDebug(message);
        }

        protected virtual void Setup()
        {
            //TODO: Make it a parameter
            _driver = new FirefoxDriver();
        }

        protected virtual void TearDown()
        {
            _driver.Quit();
        }        
    }
}
