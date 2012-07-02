using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using SiteMonitor.Core;
using SiteMonitor.Core.Log;

namespace SiteMonitor
{
    class Program
    {
        private static Monitor _monitor;
        
        static void Main(string[] args)
        {
            _monitor = new Monitor();
            
            "Initializing Site Monitor".LogInformation();
            
            var arguments = new Arguments();

            "Parsing parameters".LogInformation();

            if (CommandLineParser.Default.ParseArguments(args, arguments))
            {
                if(arguments.Monitor)
                {                    
                    var interval = arguments.Interval.HasValue ? arguments.Interval.Value : 5;                   

                    LoadTestRunners(arguments);

                    _monitor.StartMonitoring(interval);                    
                }

                if(arguments.WebInterface)
                {
                }

                Console.WriteLine("Press Enter to quit Site Monitor");
                Console.Read();

                if(arguments.Monitor)
                {
                    "Stoping test runners".LogInformation();
                    _monitor.StopAll();
                }
            }            
            
            "Site Monitor finished".LogInformation();
            Console.Read();
        }

        private static void LoadTestRunners(Arguments arguments)
        {
            "Loading test runnners".LogInformation();
            foreach (var assemblyPath in arguments.Assembly)
            {
                try
                {
                    _monitor.AddRunnersFromAssembly(assemblyPath);
                }
                catch (FileNotFoundException ex)
                {
                    ex.LogError();
                }
            }
        }
    }
}
