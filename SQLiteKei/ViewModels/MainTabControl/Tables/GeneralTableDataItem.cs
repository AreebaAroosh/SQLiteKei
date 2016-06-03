using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    public class GeneralTableDataItem
    {
        public GeneralTableDataItem()
        {
            ColumnData = new List<ColumnDataItem>();
        }

        public string Name { get; set; }

        public string TableCreateStatement { get; set; }

        public int ColumnCount { get; set; }

        public double RowCount { get; set; }

        public List<ColumnDataItem> ColumnData { get; set; }
    }
}
