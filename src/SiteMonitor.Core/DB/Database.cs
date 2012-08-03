using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using Dapper;
using SiteMonitor.Core.Reporting;
using SiteMonitor.Core.Runner;

namespace SiteMonitor.Core.DB
{
    public class Database
    {
        private string _databasePath = String.Empty;

        private string ConnectionString
        {
            get
            {
                return String.Concat("Data Source=", this._databasePath, ";Version=3;Pooling=False;Max Pool Size=20;");
            }
        }

        public Database(string databasePath)
        {
            this._databasePath = databasePath;
        }

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
                                Title,
                                YAxisLegend
                            FROM
                                RunResults
                            WHERE
                                TestName = @RunnerName
                            AND
                                RanAt BETWEEN @From AND @To
                            ORDER BY
                                RanAt ASC
                            LIMIT 5";

                var runnerInformation = conn.Query<RunResultsForRunner>(sql, new { RunnerName = runnerName, From = from, To = to }).First();

                sql = @"SELECT 
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

                runnerInformation.Entries = runResults.Select(x => new Entry() { RanAt = x.RanAt, TimeTaken = new TimeSpan(x.TicksTaken).TotalSeconds }).ToList();

                return runnerInformation;
            }
        }

        public void SaveRunResults(RunResults results)
        {
            using (var conn = CreateConnection())
            {
                var sql = @"INSERT INTO 
                                RunResults
                                (TestName,
                                Title,
                                YAxisLegend,
                                RanAt,
                                TimeTaken,
                                TimeTakenFormatted,
                                Error)
                            VALUES
                                (@TestName,
                                @Title,
                                @YAxisLegend,
                                @RanAt,
                                @TimeTaken,
                                @TimeTakenFormatted,
                                @Error)";

                conn.Execute(sql, new
                {
                    TestName = results.RunnerName,
                    Title = results.Title,
                    YAxisLegend = results.YAxisLegend,
                    RanAt = results.RanAt,
                    TimeTaken = results.TicksTaken,
                    TimeTakenFormatted = results.TimeTakenFormatted,
                    Error = results.Error
                });
            }
        }

        public void CreateDatabaseIfNeeded()
        {
            SQLiteConnection connection = null;

            if (!File.Exists(this._databasePath))
            {
                connection = new SQLiteConnection(this.ConnectionString);
                connection.Open();

                var createTable = @"CREATE TABLE RunResults (                                        
                                            TestName VARCHAR(50),
                                            Title VARCHAR(255),
                                            YAxisLegend VARCHAR(255),                                        
                                            RanAt DATETIME,
                                            TimeTaken LONG,
                                            TimeTakenFormatted VARCHAR(10),
                                            Error INTEGER
                                        )";

                connection.Execute(createTable);
            }
        }

        private DbConnection CreateConnection()
        {
            SQLiteConnection connection = new SQLiteConnection(this.ConnectionString);
            connection.Open();

            return connection;
        }

        public static string ReadDatabasePathFromConfig()
        {
            if (ConfigurationManager.AppSettings["DatabasePath"] == null)
            {
                throw new ArgumentNullException("DatabasePath", "An entry for the database file location was not found in the AppSettings");
            }

            var databasePath = ConfigurationManager.AppSettings["DatabasePath"];
            return databasePath;
        }
    }
}
