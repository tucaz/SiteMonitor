using System.Collections.Generic;
using SiteMonitor.Runner;
using SiteMonitor.DB;
using System.Threading;
using System.Diagnostics;
using System;

namespace SiteMonitor
{
    public static class Monitor
    {
        private static Timer _timer;
        
        private static List<BaseRunner> _runners = new List<BaseRunner>();
                
        public static void AddRunner(BaseRunner runner)
        {
            _runners.Add(runner);
        }

        /// <summary>
        /// Starts the monitoring with given interval
        /// </summary>
        /// <param name="interval">Interval between runs (in minutes)</param>
        public static void StartMonitoring(int interval)
        {
            _timer = new Timer(new TimerCallback(Run), null, 500, interval.ToMilliseconds());
        }

        private static void SaveResults(List<RunResults> results)
        {
            var db = new Database();
            results.ForEach(x => db.SaveRunResults(x));
        }

        private static void Run(object state)
        {
            Debug.WriteLine("Starting to run");
            Debug.WriteLine(DateTime.Now.ToString());
            
            var results = RunAllRunners();
            SaveResults(results);
        }

        private static List<RunResults> RunAllRunners()
        {
            var results = new List<RunResults>();
            
            foreach (var runner in _runners)
            {
                var result = new RunResults(runner.RunnerName);
                runner.Run();

                result.TicksTaken = runner.TimeTaken.Ticks;
                results.Add(result);
            }

            return results;
        }

        
    }
}
