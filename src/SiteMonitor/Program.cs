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

                        var webInterfacePath = String.Empty;

#if DEBUG
                        webInterfacePath = @"C:\temp\SiteMonitor"; //this was created locally with the "Publish" feature in the UI project
#else
                        webInterfacePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Web");
#endif

                        "Loading web site located in {0}".LogDebug(webInterfacePath);

                        var port = arguments.Port ?? 12345;
                        
                        _iis = new IISExpress(new Parameters
                        {
                            Path = webInterfacePath,
                            Port = port
                        });

                        ("Web interface initialized at http://localhost:" + port.ToString()).LogInformation();
                        Console.WriteLine("Press any key to finish it");
                        Console.Read();
                    }
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

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Press any key to quit Site Monitor");
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
