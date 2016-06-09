using System.Collections.Generic;
using System.IO;
using System.Linq;

using SQLiteKei.DataAccess.Database;

namespace SQLiteKei.ViewModels.MainTabControl.Databases
{
    /// <summary>
    /// A ViewModel that is used in the main tab view to display data when a database is selected.
    /// </summary>
    public class GeneralDatabaseDataItem
    {
        public GeneralDatabaseDataItem(string databasePath)
        {
            TableOverviewData = new List<TableOverviewDataItem>();
            FilePath = databasePath;

            Initialize();
        }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public int TableCount { get; set; }

        public long RowCount { get; set; }

        public List<TableOverviewDataItem> TableOverviewData { get; set; }

        private void Initialize()
        {
            var dbHandler = new DatabaseHandler(FilePath);
            var tables = dbHandler.GetTables();

            Name = dbHandler.GetDatabaseName();
            DisplayName = Path.GetFileNameWithoutExtension(FilePath);
            TableCount = tables.Count();

            foreach (var table in tables)
            {
                var tableRowCount = dbHandler.GetRowCount(table.Name);
                RowCount += tableRowCount;

                var columns = dbHandler.GetColumns(table.Name);

                TableOverviewData.Add(new TableOverviewDataItem
                {
                    ColumnCount = columns.Count,
                    Name = table.Name,
                    RowCount = tableRowCount
                });
            }
        }
    }
}
