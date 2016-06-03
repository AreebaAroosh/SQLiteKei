#region usings

using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.Generic;

#endregion

namespace SQLiteKei.ViewModels.DBTreeView
{
    public class TableItem : TreeItem
    {
        public string TableCreateStatement { get; set; }

        public int RowCount { get; set; }

        public int ColumnCount { get; set; }

        public string DatabasePath { get; set; }

        public List<ColumnItem> Columns { get; set; }
    }
}
