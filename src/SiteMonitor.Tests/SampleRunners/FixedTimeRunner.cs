using System.Diagnostics;
using System.Threading;
using OpenQA.Selenium;
using SiteMonitor.Core.Runner;

namespace SiteMonitor.Tests.SampleRunners
{
    internal class FixedTimeRunner : BaseRunner
    {
        private int _runTime;

        public FixedTimeRunner() : this(500)
        {
        }

        public FixedTimeRunner(int runTime)
        {
            this._runTime = runTime;
        }
        
        public override string RunnerName
        {
            get
            {
                return "FixedTimeRunner";
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
            Debug.WriteLine(this._runTime);
            Thread.Sleep(this._runTime);
            
            return true;
        }
    }
}
