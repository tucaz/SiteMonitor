using System;
using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using SiteMonitor.Core.Runner;

namespace SiteMonitor.Tests.SampleRunners
{
    internal class RandomTimeRunner : BaseRunner
    {        
        private int _maxRunTime;

        public RandomTimeRunner() : this(500)
        {
        }
        
        public RandomTimeRunner(int maxRunTime)
        {
            this._maxRunTime = maxRunTime;
        }

        public override string RunnerName
        {
            get
            {
                return "RandomTimeRunner";
            }
        }
        
        protected override void Setup()
        {             
        }

        protected override void TearDown()
        {            
        }
        
        protected override bool Run(IWebDriver driver)
        {
            var sleepTime = new Random(DateTime.Now.Millisecond).Next(this._maxRunTime);
            
            Debug.WriteLine(sleepTime);
            Thread.Sleep(sleepTime);
            
            return true;
        }
    }
}
