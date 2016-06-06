#region usings

using System.Data.SQLite;
using System.IO;

using NUnit.Framework;

#endregion

namespace SQLiteKei.IntegrationTests.Base
{
    public class DbTestBase
    {
        private const string testDirectory = @"C:\Test";

        [OneTimeSetUp]
        public void CreateDatabase()
        {
            if (!Directory.Exists(testDirectory))
            {
                Directory.CreateDirectory(testDirectory);
            }

            SQLiteConnection.CreateFile(testDirectory + @"\TestDB");
        }

        [OneTimeTearDown]
        public void RemoveDatabase()
        {
            if (Directory.Exists(testDirectory))
            {
                Directory.Delete(testDirectory, true);
            }
        }
    }
}
