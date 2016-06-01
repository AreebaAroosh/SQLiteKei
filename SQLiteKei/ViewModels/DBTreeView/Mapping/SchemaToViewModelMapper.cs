#region usings

using SQLiteKei.Queries.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;

#endregion

namespace SQLiteKei.ViewModels.DBTreeView.Mapping
{
    /// <summary>
    /// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
    /// </summary>
    internal class SchemaToViewModelMapper
    {
        private DbConnection connection;

        /// <summary>
        /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        /// <returns></returns>
        public DatabaseItem MapSchemaToViewModel(string databasePath)
        {
            InitializeConnection(databasePath);

            FolderItem tableFolder = MapTables();
            FolderItem indexFolder = MapIndexes();

            var databaseItem = new DatabaseItem()
            {
                DisplayName = Path.GetFileNameWithoutExtension(databasePath),
                FilePath = databasePath,
                Name = connection.Database,
                NumberOfTables = tableFolder.Items.Count
            };

            databaseItem.Items.Add(tableFolder);
            databaseItem.Items.Add(indexFolder);

            connection.Dispose();

            return databaseItem;
        }

        private void InitializeConnection(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            connection = factory.CreateConnection();

            connection.ConnectionString = string.Format("Data Source={0}", databasePath);
            connection.Open();
        }

        private FolderItem MapTables()
        {
            var tables = connection.GetSchema("Tables").AsEnumerable();
            List<TableItem> tableViewItems = GenerateTableItemsFrom(tables);

            var tableFolder = new FolderItem { DisplayName = "Tables" };

            foreach (var item in tableViewItems)
            {
                tableFolder.Items.Add(item);
            }

            return tableFolder;
        }

        private List<TableItem> GenerateTableItemsFrom(EnumerableRowCollection<DataRow> tables)
        {
            var tableViewItems = new List<TableItem>();

            foreach (var table in tables)
            {
                var command = connection.CreateCommand();
                command.CommandText = QueryBuilder
                    .Select("count(*)")
                    .From(table.ItemArray[2].ToString())
                    .Build(); 

                var result = command.ExecuteScalar();

                tableViewItems.Add(new TableItem
                {
                    DisplayName = table.ItemArray[2].ToString(),
                    TableCreateStatement = table.ItemArray[6].ToString(),
                    NumberOfRows = Convert.ToInt32(result)
                });
            }

            return tableViewItems;
        }

        private FolderItem MapIndexes()
        {
            var indexes = connection.GetSchema("Indexes").AsEnumerable();
            IEnumerable indexNames = indexes.Select(x => x.ItemArray[5]);

            var indexFolder = new FolderItem { DisplayName = "Indexes" };

            foreach (string indexName in indexNames)
            {
                indexFolder.Items.Add(new IndexItem { DisplayName = indexName });
            }

            return indexFolder;
        }
    }
}
