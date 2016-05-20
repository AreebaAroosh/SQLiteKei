using System.Collections;
using System.Data;
using System.Data.Common;
using System.IO;

namespace SQLiteKei.ViewModels.DBTreeView.Mapping
{
    internal class SchemaToViewModelMapper
    {
        public DatabaseItem MapSchemaToViewModel(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source=" + databasePath;               
                connection.Open();

                var databaseItem = new DatabaseItem()
                {
                    Name = Path.GetFileNameWithoutExtension(databasePath)
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
            IEnumerable tableNames = indexes.Select(x => x.ItemArray[5]);

            var indexFolder = new FolderItem { Name = "Indexes" };

            foreach (string tableName in tableNames)
            {
                indexFolder.Items.Add(new IndexItem { Name = tableName });
            }

            return indexFolder;
        }
    }
}
