using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using SiteMonitor.Runner;

namespace SiteMonitor.Tests.SampleRunners
{
    internal class RandomTimeRunner : BaseRunner
    {        
        private int? _maxRunTime;

        public RandomTimeRunner(int? maxRunTime = 500)
        {
            this._maxRunTime = maxRunTime;
        }

        public override string RunnerName
        {
            get
            {
                return "RandomRunner";
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
            var sleepTime = new Random(DateTime.Now.Millisecond).Next(this._maxRunTime.Value);
            
            Debug.WriteLine(sleepTime);
            Thread.Sleep(sleepTime);
            
            return true;
        }
    }
}
