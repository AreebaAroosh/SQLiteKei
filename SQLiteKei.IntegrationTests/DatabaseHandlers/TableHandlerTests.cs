using NUnit.Framework;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.IntegrationTests.Base;

using System;
using System.Data.Common;

namespace SQLiteKei.IntegrationTests.DatabaseHandlers
{
    [TestFixture, Explicit]
    public class TableHandlerTests : DbTestBase
    {
        [Test]
        public void EmptyTable_WithValidTableName_EmptiesTable()
        {
            using (var tableHandler = new TableHandler(targetDatabaseFilePath))
            {
                tableHandler.EmptyTable("TEST10");

                var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
                using (var connection = factory.CreateConnection())
                {
                    connection.ConnectionString = string.Format("Data Source={0}", targetDatabaseFilePath);
                    connection.Open();

                    var command = connection.CreateCommand();
                    command.CommandText = "SELECT count(*) FROM TEST10";
                    var result = Convert.ToInt64(command.ExecuteScalar());

                    Assert.AreEqual(0, result);
                }
            }
        }

        [Test]
        public void EmptyTable_WithInvalidTableName_ThrowsException()
        {
            using (var tableHandler = new TableHandler(targetDatabaseFilePath))
            {
                Assert.Throws(typeof(TableNotFoundException),
                    () => tableHandler.EmptyTable("TABLE_INVALID"));
            }
        }
    }
}
