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
            Columns = new List<ColumnDataItem>();
        }

        public string DatabasePath { get; set; }

        public string TableName { get; set; }

        public List<ColumnDataItem> Columns { get; set; }
    }
}
