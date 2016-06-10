using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class used for calls to the given SQLite database.
    /// </summary>
    public partial class DatabaseHandler
    {
        private DbConnection connection;

        public DatabaseHandler(string databasePath)
        {
            InitializeConnection(databasePath);
        }

        private void InitializeConnection(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            connection = factory.CreateConnection();

            connection.ConnectionString = string.Format("Data Source={0}", databasePath);
            connection.Open();
        }

        public string GetDatabaseName()
        {
            return connection.Database;
        }

        /// <summary>
        /// Gets the column meta data for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public List<Column> GetColumns(string tableName)
        {
            var columns = new List<Column>();

            using (var command = connection.CreateCommand())
            {
                command.CommandText = "PRAGMA table_info(" + tableName + ");";

                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());

                foreach (DataRow row in resultTable.Rows)
                {
                    columns.Add(new Column
                    {
                        Id = Convert.ToInt32(row.ItemArray[0]),
                        Name = (string)row.ItemArray[1],
                        DataType = (string)row.ItemArray[2],
                        IsNotNullable = Convert.ToBoolean(row.ItemArray[3]),
                        DefaultValue = row.ItemArray[4],
                        IsPrimary = Convert.ToBoolean(row.ItemArray[5])
                    });
                }
            }
            return columns;
        }

        /// <summary>
        /// Drops the specified table from the given database. Sends a plain command to the database without any further error handling.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public void DropTable(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder.Drop(tableName).Build();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Gets the row count for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public long GetRowCount(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder
                .Select("count(*)")
                .From(tableName)
                .Build();

                return Convert.ToInt64(command.ExecuteScalar());
            }
        }

        /// <summary>
        /// Gets the create statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="TableNotFoundException">Could not find table: {tableName}</exception>
        public string GetCreateStatement(string tableName)
        {
            var tables = connection.GetSchema("Tables").AsEnumerable();

            foreach(var table in tables)
            {
                if (table.ItemArray[2].Equals(tableName))
                {
                    return table.ItemArray[6].ToString();
                }
            }

            //TODO write integration test for this case
            throw new TableNotFoundException(tableName);
        }

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

        private EnumerableRowCollection<DataRow> GetSchema(string collectionName)
        {
            return connection.GetSchema(collectionName).AsEnumerable();
        }
    }
}