using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using IISExpressAutomation;
using SiteMonitor.Core;
using SiteMonitor.Core.DB;
using SiteMonitor.Core.Log;

namespace SiteMonitor
{
    class Program
    {
        private static Monitor _monitor;

        static void Main(string[] args)
        {
            "Initializing Site Monitor".LogInformation();
            _monitor = new Monitor();
            
            _monitor.InitializeDatabase(Database.ReadDatabasePathFromConfig());            
            "Database initialized".LogInformation();            

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

                        var port = arguments.Port ?? 12345;
                        _monitor.StartAPISelfHost(port);

                        ("Web interface initialized at http://localhost:" + port.ToString()).LogInformation();                        
                    }

                    Console.WriteLine("Press any key to finish it");
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
                        "Shutting web interface".LogInformation();                        
                        _monitor.StopAPISelfHost();
                    }
                }
            }

            "Site Monitor finished".LogInformation();

            Console.WriteLine(Environment.NewLine);
            Console.WriteLine("Press any key to quit Site Monitor");
            Console.Read();
        }

        private static string GetDatabasePath()
        {
            if (ConfigurationManager.AppSettings["DatabasePath"] == null)
            {
                throw new ArgumentNullException("DatabasePath", "An entry for the database file location was not found in the AppSettings");
            }

            var databasePath = ConfigurationManager.AppSettings["DatabasePath"];
            return databasePath;
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
                catch (ReflectionTypeLoadException ex)
                {
                    ex.LogError();

                    if (ex.LoaderExceptions != null)
                    {
                        ex.LoaderExceptions.ToList().ForEach(x => x.LogError());
                    }
                }
            }
        }
    }
}
