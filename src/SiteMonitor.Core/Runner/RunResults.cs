using System;

namespace SiteMonitor.Core.Runner
{
    public class RunResults
    {
        protected RunResults()
        { }

        public RunResults(string runnerName, string title, string YAxisLegend)
        {
            this.RanAt = DateTime.Now;
            this.Error = 0;
            
            this.RunnerName = runnerName;
            this.Title = title;
            this.YAxisLegend = YAxisLegend;            
        }

        public string Title { get; private set; }
        public string RunnerName { get; private set; }
        public string YAxisLegend { get; private set; }        
        public DateTime RanAt { get; private set; }
        
        public long TicksTaken { get; set; }
        public int Error { get; set; }
        
        public string TimeTakenFormatted
        {
            get
            {
                var dt = new DateTime(this.TicksTaken);
                return dt.ToString();
            }
        }
    }
}
