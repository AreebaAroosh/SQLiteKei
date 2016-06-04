#region usings

using SQLiteKei.DataAccess.Models;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;

#endregion

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class used for calls to the given SQLite database.
    /// </summary>
    public class DatabaseHandler
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
                        Type = (string)row.ItemArray[2],
                        IsNotNullable = Convert.ToBoolean(row.ItemArray[3]),
                        DefaultValue = row.ItemArray[4],
                        IsPrimary = Convert.ToBoolean(row.ItemArray[5])
                    });
                }
            }

            return columns;
        }


    }
}