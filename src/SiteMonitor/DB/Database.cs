using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteMonitor.Runner;
using System.Data.SQLite;
using System.Data.Common;
using Dapper;
using System.IO;
using SiteMonitor.Reporting;

namespace SiteMonitor.DB
{
    public class Database
    {
        private static bool _databaseCreated = false;
        private static volatile object _syncLock = new object();

        public List<string> GetAllRunners()
        {
            using (var conn = CreateConnection())
            {
                var sql = @"SELECT 
                                    DISTINCT TestName
                                 FROM
                                    RunResults
                                 ORDER BY
                                    TestName ASC";

                var runners = conn.Query<string>(sql);

                return runners.ToList();
            }
        }

        public RunResultsForRunner GetRunResults(string runnerName, DateTime from, DateTime to)
        {
            if (String.IsNullOrWhiteSpace(runnerName)) throw new ArgumentNullException("runnerName");

            using (var conn = CreateConnection())
            {
                var sql = @"SELECT 
                                        TestName as RunnerName,
                                        RanAt,
                                        TimeTaken as TicksTaken
                                     FROM
                                        RunResults
                                     WHERE
                                        TestName = @RunnerName
                                     AND
                                        RanAt BETWEEN @From AND @To
                                     ORDER BY
                                        RanAt ASC";

                var runResults = conn.Query<RunResults>(sql, new { RunnerName = runnerName, From = from, To = to });

                var results = new RunResultsForRunner()
                {
                    Entries = runResults.Select(x => new Entry() { RanAt = x.RanAt, TimeTaken = new TimeSpan(x.TicksTaken).TotalSeconds }).ToList()
                };

                return results;
            }
        }

        public void SaveRunResults(RunResults results)
        {
            using (var conn = CreateConnection())
            {
                var sql = @"INSERT INTO 
                                RunResults
                                (TestName,
                                RanAt,
                                TimeTaken,
                                TimeTakenFormatted)
                            VALUES
                                (@TestName,
                                @RanAt,
                                @TimeTaken,
                                @TimeTakenFormatted)";

                conn.Execute(sql, new { TestName = results.RunnerName, RanAt = results.RanAt, TimeTaken = results.TicksTaken, TimeTakenFormatted = results.TimeTakenFormatted });
            }
        }

        private static DbConnection CreateConnection()
        {
            var databaseLocation = @"C:\temp\WebMonitor.sqlite";
            var connectionString = "Data Source=" + databaseLocation + ";Version=3;Pooling=False;Max Pool Size=20;";
            SQLiteConnection connection = null;

            if (!_databaseCreated)
            {
                lock (_syncLock)
                {
                    if (!File.Exists(databaseLocation))
                    {
                        connection = new SQLiteConnection(connectionString);
                        connection.Open();

                        var createTable = @"CREATE TABLE RunResults (                                        
                                        TestName VARCHAR(50),
                                        RanAt DATETIME,
                                        TimeTaken LONG,
                                        TimeTakenFormatted VARCHAR(10)
                                    )";

                        connection.Execute(createTable);
                    }
                    else
                    {
                        connection = new SQLiteConnection(connectionString);
                        connection.Open();
                    }

                    _databaseCreated = true;
                }
            }
            else
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();
            }

            return connection;
        }
    }
}
