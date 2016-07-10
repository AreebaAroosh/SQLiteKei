using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using SQLiteKei.DataAccess.Database;

namespace SQLiteKei.ViewModels.MainTabControl.Databases
{
    /// <summary>
    /// A ViewModel that is used in the main tab view to display data when a database is selected.
    /// </summary>
    public class GeneralDatabaseViewModel
    {
        public GeneralDatabaseViewModel(string databasePath)
        {
            TableOverviewData = new List<TableOverviewDataItem>();
            FilePath = databasePath;

            Initialize();
        }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public string FileSize { get; set; }

        public int TableCount { get; set; }

        public long RowCount { get; set; }

        public List<TableOverviewDataItem> TableOverviewData { get; set; }

        private void Initialize()
        {
            using (var dbHandler = new DatabaseHandler(FilePath))
            using (var tableHandler = new TableHandler(FilePath))
            {
                var tables = dbHandler.GetTables();

                Name = dbHandler.GetDatabaseName();
                DisplayName = Path.GetFileNameWithoutExtension(FilePath);
                TableCount = tables.Count();

                foreach (var table in tables)
                {
                    var tableRowCount = tableHandler.GetRowCount(table.Name);
                    RowCount += tableRowCount;

                    var columns = tableHandler.GetColumns(table.Name);

                    TableOverviewData.Add(new TableOverviewDataItem
                    {
                        ColumnCount = columns.Count,
                        Name = table.Name,
                        RowCount = tableRowCount
                    });
                }

                var fileInfo = new FileInfo(FilePath);
                FileSize = GetSize(fileInfo.Length);
            }
        }

        static readonly string[] SizeSuffixes = { "Bytes", "KB", "MB", "GB", "TB" };

        private string GetSize(long value)
        {
            int suffix = 0;
            decimal decimalValue = (decimal)value;

            while(Math.Round(decimalValue / 1024) >= 1)
            {
                decimalValue /= 1024;
                suffix++;
            }

            return string.Format("{0} {1}", Math.Round(decimalValue, 2), SizeSuffixes[suffix]);
        }
    }
}
