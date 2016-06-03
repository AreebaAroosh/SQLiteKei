using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    public class GeneralTableDataItem
    {
        public GeneralTableDataItem()
        {
            ColumnData = new List<ColumnOverviewDataItem>();
        }

        public string Name { get; set; }

        public string TableCreateStatement { get; set; }

        public int ColumnCount { get; set; }

        public double RowCount { get; set; }

        public List<ColumnOverviewDataItem> ColumnData { get; set; }
    }
}
