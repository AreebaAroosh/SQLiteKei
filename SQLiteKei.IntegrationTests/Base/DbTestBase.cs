using NUnit.Framework;

using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace SQLiteKei.IntegrationTests.Base
{
    public class DbTestBase
    {
        protected const string DATABASEFILENAME = "TestDb";

        protected string testDirectory;

        protected string targetDatabaseFilePath;

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            testDirectory = Path.Combine(Path.GetTempPath(), "SQLiteKei");

            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true); 
            }

            Directory.CreateDirectory(testDirectory);

            targetDatabaseFilePath = Path.Combine(testDirectory, DATABASEFILENAME);
            SQLiteConnection.CreateFile(targetDatabaseFilePath);
        }

        [SetUp]
        public void ResetFakeData()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = string.Format("Data Source={0}", targetDatabaseFilePath);
                connection.Open();

                for (var i = 1; i <= 10; i++)
                {
                    var dropCommand = connection.CreateCommand();
                    dropCommand.CommandText = string.Format("DROP TABLE IF EXISTS TEST{0}", i);
                    dropCommand.ExecuteNonQuery();

                    var createCommand = connection.CreateCommand();
                    createCommand.CommandText = string.Format("CREATE TABLE TEST{0} (ColumnA{0} varchar({1}), ColumnB{0} int)", i, i+50);
                    createCommand.ExecuteNonQuery();

                    for (var j = 1; j <= i; j++)
                    {
                        var insertCommand = connection.CreateCommand();
                        insertCommand.CommandText = string.Format("INSERT INTO TEST{0} VALUES('ENTRY{1}', '{1}')", i, j);
                        insertCommand.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }
        }
    }
}
