using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using SiteMonitor.DB;
using SiteMonitor.Runner;

namespace SiteMonitor
{
    public static class Monitor
    {
        private static Timer _timer;
        
        private static List<BaseRunner> _runners = new List<BaseRunner>();
                
        /// <summary>
        /// Add a test runner to the runner's pool that will be monitored
        /// </summary>
        /// <param name="runner">An instance of BaseRunner</param>
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

        /// <summary>
        /// Stops all runners
        /// </summary>
        public static void StopAll()
        {
            _timer.Dispose();
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
            
            Parallel.ForEach(_runners, x => {
                Debug.WriteLine(DateTime.Now.ToString() + " - Running: " + x.RunnerName);
                
                var result = new RunResults(x.RunnerName);
                x.Run();

                result.TicksTaken = x.TimeTaken.Ticks;
                results.Add(result);
            });
         
            return results;
        }

        
    }
}
