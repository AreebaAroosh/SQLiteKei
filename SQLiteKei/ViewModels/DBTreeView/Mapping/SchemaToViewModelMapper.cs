﻿#region usings

using System.Collections;
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
        /// <summary>
        /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        /// <returns></returns>
        public DatabaseItem MapSchemaToViewModel(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = string.Format("Data Source={0}", databasePath);               
                connection.Open();

                FolderItem tableFolder = MapTables(connection.GetSchema("Tables"));
                FolderItem indexFolder = MapIndexes(connection.GetSchema("Indexes"));

                var databaseItem = new DatabaseItem()
                {
                    DisplayName = Path.GetFileNameWithoutExtension(databasePath),
                    FilePath = databasePath,
                    Name = connection.Database,
                    NumberOfTables = tableFolder.Items.Count
                };

                databaseItem.Items.Add(tableFolder);
                databaseItem.Items.Add(indexFolder);

                return databaseItem;
            }
        }

        private FolderItem MapTables(DataTable schema)
        {
            var tables = schema.AsEnumerable();

            var tableViewItems = tables.Select(x => new TableItem
            {
                DisplayName = x.ItemArray[2].ToString(),
                TableCreateStatement = x.ItemArray[6].ToString()
            });

            var tableFolder = new FolderItem { DisplayName = "Tables" };

            foreach (var item in tableViewItems)
            {
                tableFolder.Items.Add(item);
            }

            return tableFolder;
        }

        private FolderItem MapIndexes(DataTable schema)
        {
            var indexes = schema.AsEnumerable();
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
