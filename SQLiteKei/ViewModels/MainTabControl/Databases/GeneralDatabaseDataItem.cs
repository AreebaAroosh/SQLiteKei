using System.Collections.Generic;

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
        }

        public string DisplayName { get; set; }

        public string Name { get; set; }

        public string FilePath { get; set; }

        public int TableCount { get; set; }

        public double RowCount { get; set; }

        public List<TableOverviewDataItem> TableOverviewData { get; set; }
    }
}
