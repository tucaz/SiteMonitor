using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using IISExpressAutomation;
using SiteMonitor.Core;
using SiteMonitor.Core.Log;

namespace SiteMonitor
{
    class Program
    {
        private static Monitor _monitor;
        private static IISExpress _iis;

        static void Main(string[] args)
        {
            _monitor = new Monitor();

            "Initializing Site Monitor".LogInformation();

            var arguments = new Arguments();

            "Parsing parameters".LogInformation();

            if (CommandLineParser.Default.ParseArguments(args, arguments))
            {
                try
                {
                    if (arguments.Monitor)
                    {
                        var interval = arguments.Interval.HasValue ? arguments.Interval.Value : 5;
                        
                        LoadTestRunners(arguments);

                        _monitor.StartMonitoring(interval);
                    }

                    if (arguments.WebInterface)
                    {
                        "Initializing web interface".LogInformation();

                        var webInterfacePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web");

                        "Loading web site located in {0}".LogDebug(webInterfacePath);
                        
                        _iis = new IISExpress(new Parameters
                        {
                            Path = webInterfacePath,
                            Port = 667
                        });

                    }

                    Console.WriteLine("Press Enter to quit Site Monitor");
                    Console.Read();
                }
                finally
                {
                    if (arguments.Monitor)
                    {
                        "Stoping test runners".LogInformation();
                        _monitor.StopAll();
                    }

                    if (arguments.WebInterface)
                    {
                        "Shutting down IIS Express".LogInformation();
                        _iis.Dispose();
                    }
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
