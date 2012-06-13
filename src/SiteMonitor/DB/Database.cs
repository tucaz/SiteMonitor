using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SiteMonitor.Runner;
using System.Data.SQLite;
using System.Data.Common;
using Dapper;
using System.IO;

namespace SiteMonitor.DB
{
    public class Database
    {
        private static bool _databaseCreated = false;

        public void GetRunResults(string runnerName, DateTime from, DateTime to)
        {
            using (var conn = CreateConnection())
            {                
                var sqlScale = @"SELECT 
                                    MIN(TimeTaken) as Lower,
                                    MAX(TimeTaken) as Upper
                                 FROM
                                    RunResults
                                 WHERE
                                    TestName = @RunnerName
                                 AND
                                    RanAt BETWEEN @From AND @To";

                var sqlRunResults = @"SELECT 
                                        TestName as RunnerName,
                                        RanAt,
                                        TimeTaken as TicksTaken
                                     FROM
                                        RunResults
                                     WHERE
                                        TestName = @RunnerName
                                     AND
                                        RanAt BETWEEN @From AND @To";

                var scale = conn.Query(sqlScale, new { RunnerName = runnerName, From = from, To = to }).First();
                var results = conn.Query<RunResults>(sqlRunResults, new { RunnerName = runnerName, From = from, To = to });
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
            else
            {
                connection = new SQLiteConnection(connectionString);
                connection.Open();
            }

            return connection;
        }
    }
}
