using NUnit.Framework;

using System.Data.Common;
using System.Data.SQLite;
using System.IO;

namespace SQLiteKei.IntegrationTests.Base
{
    public class DbTestBase
    {
        private const string TESTDIRECTORY = @"C:\Test";

        private const string FILENAME = "TestDb";

        private string targetFilePath;

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            if (Directory.Exists(TESTDIRECTORY))
            {
                Directory.Delete(TESTDIRECTORY, true); 
            }

            Directory.CreateDirectory(TESTDIRECTORY);

            targetFilePath = Path.Combine(TESTDIRECTORY, FILENAME);
            SQLiteConnection.CreateFile(targetFilePath);
        }

        [SetUp]
        public void ResetFakeData()
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = string.Format("Data Source={0}", targetFilePath);
                connection.Open();

                for (var i = 1; i <= 10; i++)
                {
                    var dropCommand = connection.CreateCommand();
                    dropCommand.CommandText = string.Format("DROP TABLE IF EXISTS TEST{0}", i);
                    dropCommand.ExecuteNonQuery();

                    var createCommand = connection.CreateCommand();
                    createCommand.CommandText = string.Format("CREATE TABLE TEST{0} (ColumnA{0} varchar({0}), ColumnB{0} int)", i);
                    createCommand.ExecuteNonQuery();
                }

                connection.Close();
            }
        }
    }
}
