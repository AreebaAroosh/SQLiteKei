#region usings

using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Windows;

#endregion

namespace SQLiteKei.ViewModels.DBTreeView.Mapping
{
    /// <summary>
    /// A mapping class that opens a connection to the provided database and builds a hierarchical ViewModel structure.
    /// </summary>
    internal class SchemaToViewModelMapper
    {
        private DbConnection connection;

        private string databasePath;

        /// <summary>
        /// Maps the provided database to a hierarchical ViewModel structure with a DatabaseItem as its root.
        /// </summary>
        /// <param name="databasePath">The database path.</param>
        /// <returns></returns>
        public DatabaseItem MapSchemaToViewModel(string databasePath)
        {
            Application.Current.Properties["CurrentDatabase"] = databasePath;
            this.databasePath = databasePath;
            InitializeConnection(databasePath);

            TableFolderItem tableFolder = MapTables();
            FolderItem viewFolder = MapViews();
            FolderItem indexFolder = MapIndexes();
            FolderItem triggerFolder = MapTriggers();

            var databaseItem = new DatabaseItem()
            {
                DisplayName = Path.GetFileNameWithoutExtension(databasePath),
                DatabasePath = databasePath,
                Name = connection.Database,
                NumberOfTables = tableFolder.Items.Count
            };

            databaseItem.Items.Add(tableFolder);
            databaseItem.Items.Add(viewFolder);
            databaseItem.Items.Add(indexFolder);
            databaseItem.Items.Add(triggerFolder);

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

        private TableFolderItem MapTables()
        {
            var tables = connection.GetSchema("Tables").AsEnumerable();

            List<TableItem> tableViewItems = GenerateTableItemsFrom(tables);

            var tableFolder = new TableFolderItem { DisplayName = "Tables" };

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
                var tableName = table.ItemArray[2].ToString();

                List<ColumnItem> columns = MapColumnsFor(tableName);

                tableViewItems.Add(new TableItem
                {
                    DisplayName = tableName,
                    TableCreateStatement = table.ItemArray[6].ToString(),
                    RowCount = GetRowCountFor(tableName),
                    DatabasePath = databasePath,
                    ColumnCount = columns.Count,
                    Columns = columns
                });
            }

            return tableViewItems;
        }

        private List<ColumnItem> MapColumnsFor(string tableName)
        {
            var databaseHandler = new DatabaseHandler(databasePath);
            var columns = databaseHandler.GetColumns(tableName);

            var columnItems = new List<ColumnItem>();

            foreach (var column in columns)
            {
                columnItems.Add(new ColumnItem
                {
                    DisplayName = column.Name,
                    DatabasePath = databasePath
                });
            }

            return columnItems;
        }

        private int GetRowCountFor(string tableName)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = QueryBuilder
                .Select("count(*)")
                .From(tableName)
                .Build();

                return Convert.ToInt32(command.ExecuteScalar());
            }
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

        private FolderItem MapTriggers()
        {
            var triggers = connection.GetSchema("Triggers").AsEnumerable();
            IEnumerable triggerNames = triggers.Select(x => x.ItemArray[3]);

            var triggerFolder = new FolderItem { DisplayName = "Triggers" };

            foreach (string triggerName in triggerNames)
            {
                triggerFolder.Items.Add(new TriggerItem
                {
                    DisplayName = triggerName,
                    DatabasePath = databasePath
                });
            }

            return triggerFolder;
        }

        private FolderItem MapViews()
        {
            var views = connection.GetSchema("Views").AsEnumerable();
            IEnumerable viewNames = views.Select(x => x.ItemArray[2]);

            var viewFolder = new FolderItem { DisplayName = "Views" };

            foreach (string viewName in viewNames)
            {
                viewFolder.Items.Add(new ViewItem
                {
                    DisplayName = viewName,
                    DatabasePath = databasePath
                });
            }

            return viewFolder;
        }
    }
}
