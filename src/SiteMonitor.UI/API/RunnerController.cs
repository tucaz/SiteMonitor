using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using SiteMonitor.Core;
using SiteMonitor.Core.DB;
using SiteMonitor.Core.Reporting;

namespace SiteMonitor.API
{
    public class RunnerController : ApiController
    {
        private Database _db;

        public RunnerController()
        {
            _db = new Database(Database.ReadDatabasePathFromConfig());
        }

        public IEnumerable<string> Get()
        {
            var runners = _db.GetAllRunners();

            return runners;
        }

        public dynamic GetResults(string runnerName, string from, string to, string interval)
        {
            RunResultsForRunner results = null;

            DateTime fromDate = DateTime.MinValue;
            DateTime toDate = DateTime.MinValue;

            ExtractParameters(from, to, interval, out fromDate, out toDate);

            results = _db.GetRunResults(runnerName, fromDate, toDate);

            var @return = new
            {
                Title = results.Title,
                YAxisLegend = results.YAxisLegend,
                Entries = results.Entries.Select(x =>
                    new dynamic[2] { x.RanAt.ToString("hh:mm:ss"), Math.Round(x.TimeTaken, 2) }).ToArray()
            };

            return @return;
        }

        private void ExtractParameters(string from, string to, string interval, out DateTime fromDate, out DateTime toDate)
        {
            if (!String.IsNullOrWhiteSpace(interval))
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