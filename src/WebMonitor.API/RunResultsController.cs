using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using SiteMonitor.DB;
using SiteMonitor.Reporting;
using SiteMonitor.Runner;

namespace WebMonitor.API
{
    public class RunResultsController : ApiController
    {
        public dynamic[][] Get(string runnerName, DateTime? from, DateTime? to)
        {
            var db = new Database();

            var results = db.GetRunResults(runnerName, from, to)
                .Entries
                .Select(x => 
                    new dynamic[2] { x.RanAt.ToString("hh:MM:ss"), Math.Round(x.TimeTaken, 2) }
                    ).ToList();

            return results.ToArray();
        }
    }
}