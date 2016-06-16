using SQLiteKei.DataAccess.Models;

using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class used for calls to the given SQLite database.
    /// </summary>
    public partial class DatabaseHandler : DisposableDbHandler
    {
        public static void CreateDatabase(string filePath)
        {
            SQLiteConnection.CreateFile(filePath);
        }

        public DatabaseHandler(string databasePath) : base(databasePath)
        {
        }

        /// <summary>
        /// Returns the name of the current database.
        /// </summary>
        /// <returns></returns>
        public string GetDatabaseName()
        {
            return connection.Database;
        }

        /// <summary>
        /// Returns information about all tables in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Table> GetTables()
        {
            var dataRows = GetSchema("Tables");
            var tables = new List<Table>();

            foreach (var row in dataRows)
            {
                tables.Add(new Table
                {
                    DatabaseName = row.ItemArray[0].ToString(),
                    Name = row.ItemArray[2].ToString(),
                    CreateStatement = row.ItemArray[6].ToString(),
                });
            }

            return tables;
        }

        /// <summary>
        /// Returns information about all views in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<View> GetViews()
        {
            var dataRows = GetSchema("Views");
            var views = new List<View>();

            foreach (var row in dataRows)
            {
                views.Add(new View
                {
                    Name = row.ItemArray[2].ToString()
                });
            }

            return views;
        }

        /// <summary>
        /// Returns information about all indexes in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Index> GetIndexes()
        {
            var dataRows = GetSchema("Indexes");
            var indexes = new List<Index>();

            foreach (var row in dataRows)
            {
                indexes.Add(new Index
                {
                    Name = row.ItemArray[5].ToString()
                });
            }

            return indexes;
        }

        /// <summary>
        /// Returns information about all triggers in the current database.
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Trigger> GetTriggers()
        {
            var dataRows = GetSchema("Triggers");
            var triggers = new List<Trigger>();

            foreach (var row in dataRows)
            {
                triggers.Add(new Trigger
                {
                    Name = row.ItemArray[2].ToString()
                });
            }

            return triggers;
        }

        private IEnumerable<DataRow> GetSchema(string collectionName)
        {
            return connection.GetSchema(collectionName).AsEnumerable();
        }
    }
}