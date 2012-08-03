using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Reflection;
using System.Text;
using System.Web.Http;

namespace SiteMonitor.Core.API
{
    public class StaticFileController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string folder, string file)
        {
            file = "SiteMonitor.Core.API.static." + folder + "." + file.Replace('/', '.');

            using (var fs = Assembly.GetExecutingAssembly().GetManifestResourceStream(file))
            {
                HttpResponseMessage response = null;

                if (fs == null)
                {
                    response = new HttpResponseMessage(HttpStatusCode.NotFound);
                }
                else
                {                   
                    
                    response = new HttpResponseMessage();                    
                    response.Content = new ByteArrayContent(UTF8Encoding.UTF8.GetBytes(new StreamReader(fs).ReadToEnd()));                    
                }
                
                return response;
            }
        }
    }
}
