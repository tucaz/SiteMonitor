using System.Collections.Generic;
using SiteMonitor.Runner;
using SiteMonitor.DB;

namespace SiteMonitor
{
    public class Monitor
    {
        private static List<BaseRunner> _runners = new List<BaseRunner>();
        
        public static void AddRunner(BaseRunner runner)
        {
            _runners.Add(runner);
        }

        public static void StartMonitoring()
        {
            var results = StartAllRunners();
            SaveResults(results);
        }

        private static void SaveResults(List<RunResults> results)
        {
            var db = new Database();
            results.ForEach(x => db.SaveRunResults(x));
        }

        private static List<RunResults> StartAllRunners()
        {
            var results = new List<RunResults>();
            
            foreach (var runner in _runners)
            {
                var result = new RunResults(runner.RunnerName);
                runner.Run();

                result.TicksTaken = runner.TimeTaken.Value.Ticks;
                results.Add(result);
            }

            return results;
        }

        
    }
}
