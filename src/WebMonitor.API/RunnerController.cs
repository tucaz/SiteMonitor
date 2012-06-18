using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using SiteMonitor.DB;

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
    }
}