using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiteMonitor.DB;
using SiteMonitor.Reporting;
using SiteMonitor;

namespace WebMonitor.API
{
    public class RunnerController : ApiController
    {
        public IEnumerable<string> Get()
        {
            var db = new Database();
            var runners = db.GetAllRunners();

            return runners;
        }

        public dynamic[][] GetResults(string runnerName, string from, string to, string interval)
        {
            var db = new Database();            
            RunResultsForRunner results = null;

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            ExtractParameters(from, to, interval, out fromDate, out toDate);

            results = db.GetRunResults(runnerName, fromDate, toDate);

            var @return = results.Entries.Select(x => 
                new dynamic[2] { x.RanAt.ToString("hh:mm:ss"), Math.Round(x.TimeTaken, 2) }).ToArray();

            return @return;
        }

        private void ExtractParameters(string from, string to, string interval, out DateTime fromDate, out DateTime toDate)
        {
            if(!String.IsNullOrWhiteSpace(interval))
            {
                fromDate = interval.FromInterval();
                toDate = DateTime.Now;
            }
            else
            {
                fromDate = Convert.ToDateTime(from);
                toDate = Convert.ToDateTime(to);
            }                      
        }
    }
}