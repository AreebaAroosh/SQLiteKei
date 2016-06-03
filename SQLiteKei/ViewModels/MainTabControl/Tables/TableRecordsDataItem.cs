using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    /// <summary>
    /// A ViewModel that is used in the table records tab.
    /// </summary>
    public class TableRecordsDataItem
    {
        public TableRecordsDataItem()
        {
            Columns = new List<ColumnOverviewDataItem>();
        }

        public string DatabasePath { get; set; }

        public string TableName { get; set; }

        public List<ColumnOverviewDataItem> Columns { get; set; }
    }
}
