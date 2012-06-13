using System;

namespace SiteMonitor.Runner
{
    public class RunResults
    {
        protected RunResults()
        {}
        
        public RunResults(string runnerName)
        {
            this.RanAt = DateTime.Now;
            this.RunnerName = runnerName;
        }

        public string RunnerName { get; private set; }
        public DateTime RanAt { get; private set; }
        public long TicksTaken { get; set; }
        public string TimeTakenFormatted
        {
            get
            {
                var dt = new DateTime(this.TicksTaken);
                var timeTaken = (dt.Minute + (1m / dt.Second)).ToString("N2");

                return timeTaken;
            }
        }
    }
}
