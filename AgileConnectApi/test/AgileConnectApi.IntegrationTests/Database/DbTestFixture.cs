using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
//using DapperWrapper;
using DbUp;
using System.Reflection;

namespace AgileConnectApi.IntegrationTests.Database
{
    public class DbTestFixture : IDisposable
    {
        private static string _testAssemblyLocation;
        private static string _dbPath;
        private readonly string _scriptsFolder;
        private static string fileName = "Test_data";
        private static string testDbFolder = "TestDb";
        private static string server = @"server=(LocalDB)\MSSQLLocalDB";

        
        public DbTestFixture()
        {
            _testAssemblyLocation =new FileInfo(typeof(DbTestFixture).GetTypeInfo().Assembly.Location).Directory.FullName;
            _dbPath = Path.Combine(_testAssemblyLocation, testDbFolder);
            _scriptsFolder = Path.Combine(_testAssemblyLocation, @"..\..\..\sql");

            CreateTestDb();
        }

        public void Dispose()
        {

        }

        public string ConnectionString { get { return string.Format(@"{0}; Timeout = 60; AttachDBFilename ={1}\{2}.mdf;", server, _dbPath, fileName); } }

        public SqlConnection GetNewOpenConnection()
        {
            var constring = ConnectionString;

            var con = new SqlConnection(constring);
            con.Open();
            return con;
        }

        private SqlConnection GetNewOpenServerConnection()
        {
            var con = new SqlConnection(server);
            con.Open();
            return con;
        }


        private void CreateTestDb()
        {
            EnsureDbDirectoryExists();

            using (SqlConnection connection = GetNewOpenServerConnection())
            {
                DetachDatabase(connection);

                //clearfile
                var mdf = string.Format(@"{0}\{1}.mdf", _dbPath, fileName);
                var ldf = string.Format(@"{0}\{1}.ldf", _dbPath, fileName);

                DeleteDatabaseFiles(mdf, ldf);
                CreateNewTestDatabase(connection, mdf, ldf);
            }

            BuildSchema();

        }

        private static void CreateNewTestDatabase(SqlConnection connection, string mdf, string ldf)
        {
            string create = string.Format(@"
                                    CREATE DATABASE
                                        [Test]
                                    ON PRIMARY (
                                       NAME=Test_data,
                                       FILENAME = '{0}'
                                    )
                                    LOG ON (
                                        NAME=Test_log,
                                        FILENAME = '{1}'
                                    )"
                                , mdf, ldf);



            SqlCommand command = new SqlCommand(create, connection);
            command.ExecuteNonQuery();
        }

        private static void DeleteDatabaseFiles(string mdf, string ldf)
        {
            if (File.Exists(mdf))
                File.Delete(mdf);

            if (File.Exists(ldf))
                File.Delete(ldf);
        }

        private static void DetachDatabase(SqlConnection connection)
        {
            var detach = @"DECLARE @db_id int;  
                              SET @db_id = DB_ID(N'Test');  
                              IF @db_id IS NOT NULL   
                                BEGIN;
                                    EXEC sp_detach_db N'Test', true;
                                END;";

            SqlCommand commanddetach = new SqlCommand(detach, connection);
            commanddetach.ExecuteNonQuery();
        }

        private static void EnsureDbDirectoryExists()
        {
            var dir = new FileInfo(typeof(DbTestFixture).GetTypeInfo().Assembly.Location).Directory.FullName;
            var _dbPath = Path.Combine(dir, testDbFolder);

            if (!Directory.Exists(_dbPath))
                Directory.CreateDirectory(_dbPath);
        }

        private void BuildSchema()
        {
            var connectionString = ConnectionString;
            
            var upgrader = DeployChanges.To
                    .SqlDatabase(connectionString)
                    //.WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                    .WithScriptsFromFileSystem(_scriptsFolder)
                    .LogToConsole()
                    .Build();

            var result = upgrader.PerformUpgrade();

            if (!result.Successful)
            {
                throw new Exception("unable to migrate: " + result.Error);
            }
        }

        public void ClearRecords()
        {
            string[] tables = new string[] { "Participant" };

            List<Task> tasks = new List<Task>(tables.Length);

            using (var con = GetNewOpenConnection())
            {
                foreach (var table in tables)
                {
                    using (var cmd = new SqlCommand("DELETE FROM " + table, con))
                    {
                        tasks.Add(cmd.ExecuteNonQueryAsync());
                    }
                }

                Task.WaitAll(tasks.ToArray());
            }


        }
    }
}