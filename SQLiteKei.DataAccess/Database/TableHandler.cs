using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.DataAccess.QueryBuilders;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;

namespace SQLiteKei.DataAccess.Database
{
    public class TableHandler : DisposableDbHandler
    {
        public TableHandler(string databasePath) : base(databasePath)
        {
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
        /// Gets the create statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        /// <exception cref="TableNotFoundException">Could not find table: {tableName}</exception>
        public string GetCreateStatement(string tableName)
        {
            var tables = connection.GetSchema("Tables").AsEnumerable();

            foreach (var table in tables)
            {
                if (table.ItemArray[2].Equals(tableName))
                {
                    return table.ItemArray[6].ToString();
                }
            }

            throw new TableNotFoundException(tableName);
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
        /// Deletes all rows from the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        public void EmptyTable(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = string.Format("DELETE FROM {0}", tableName);

                try
                {
                    command.ExecuteNonQuery();
                }
                catch(SQLiteException ex)
                {
                    if(ex.Message.Contains("no such table"))
                    {
                        throw new TableNotFoundException(tableName);
                    }
                    throw;
                }
            }
        }
    }
}
