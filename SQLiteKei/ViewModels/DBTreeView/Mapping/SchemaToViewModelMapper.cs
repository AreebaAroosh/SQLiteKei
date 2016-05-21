#region usings

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

                var databaseItem = new DatabaseItem()
                {
                    Name = Path.GetFileNameWithoutExtension(databasePath),
                    FilePath = databasePath
                };

                FolderItem tableFolder = MapTables(connection.GetSchema("Tables"));
                FolderItem indexFolder = MapIndexes(connection.GetSchema("Indexes"));

                databaseItem.Items.Add(tableFolder);
                databaseItem.Items.Add(indexFolder);

                return databaseItem;
            }
        }

        private FolderItem MapTables(DataTable schema)
        {
            var tables = schema.AsEnumerable();
            IEnumerable tableNames = tables.Select(x => x.ItemArray[2]);

            var tableFolder = new FolderItem { Name = "Tables" };

            foreach (string tableName in tableNames)
            {
                tableFolder.Items.Add(new TableItem { Name = tableName });
            }

            return tableFolder;
        }

        private FolderItem MapIndexes(DataTable schema)
        {
            var indexes = schema.AsEnumerable();
            IEnumerable indexNames = indexes.Select(x => x.ItemArray[5]);

            var indexFolder = new FolderItem { Name = "Indexes" };

            foreach (string indexName in indexNames)
            {
                indexFolder.Items.Add(new IndexItem { Name = indexName });
            }

            return indexFolder;
        }
    }
}
