using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using SiteMonitor.Core.DB;
using SiteMonitor.Core.Runner;
using SiteMonitor.Core.Log;

namespace SiteMonitor.Core
{
    public class Monitor
    {
        private Timer _timer;

        private List<BaseRunner> _runners = new List<BaseRunner>();
        public List<BaseRunner> RunnersLoaded
        {
            get
            {
                return _runners;
            }
        }

        /// <summary>
        /// Add a test runner to the runner's pool that will be monitored
        /// </summary>
        /// <param name="runner">An instance of BaseRunner</param>
        public void AddRunner(BaseRunner runner)
        {
            _runners.Add(runner);
        }

        /// <summary>
        /// Scans the informed assembly looking for test runners inheriting from BaseRunner
        /// </summary>
        /// <param name="assemblyPath">Full path to the assembly</param>
        public void AddRunnersFromAssembly(string assemblyPath)
        {
            if (!File.Exists(assemblyPath))
                throw new FileNotFoundException("Assembly not found", assemblyPath);

            var testRunners = Assembly.LoadFrom(assemblyPath)
                .GetTypes().Where(x => x.BaseType == typeof(BaseRunner))
                .Select(x => (BaseRunner)Activator.CreateInstance(x)).ToList();

            testRunners.ForEach(AddRunner);

            "Test runners found:".LogInformation();
            testRunners.ForEach(x => "\t\t{0}".LogInformation(x.RunnerName));
        }

        /// <summary>
        /// Starts the monitoring with given interval
        /// </summary>
        /// <param name="interval">Interval between runs (in minutes)</param>
        public void StartMonitoring(int interval)
        {
            "Using {0} minutes as interval".LogInformation(interval);

            "Starting monitoring".LogInformation();
            _timer = new Timer(new TimerCallback(Run), null, 500, interval.ToMilliseconds());
        }

        private void SaveResults(List<RunResults> results)
        {
            "Persisting results".LogInformation();
            var db = new Database();
            results.ForEach(x => db.SaveRunResults(x));
            "Results saved".LogInformation();
        }

        /// <summary>
        /// Stops all runners
        /// </summary>
        public void StopAll()
        {
            _timer.Dispose();
        }

        private void Run(object state)
        {
            "Starting to run".LogInformation();

            var results = RunAllRunners();
            SaveResults(results);
        }

        private List<RunResults> RunAllRunners()
        {
            var results = new List<RunResults>();

            Parallel.ForEach(_runners, x =>
            {
                "Running {0}".LogInformation(x.RunnerName);

                var result = new RunResults(x.RunnerName);
                x.Run();

                result.TicksTaken = x.TimeTaken.Ticks;
                results.Add(result);

                "Finished running {0}. Time taken: {1}ms".LogInformation(x.RunnerName, x.TimeTaken.TotalMilliseconds);
            });

            return results;
        }
    }
}
