using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using SiteMonitor.Core;

namespace SiteMonitor
{
    class Program
    {
        static void Main(string[] args)
        {
            var arguments = new Arguments();

            if (CommandLineParser.Default.ParseArguments(args, arguments))
            {

            }
        }
    }
}
