using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.ViewModels.DBTreeView
{
    public class TableItem : TreeItem
    {
        public string TableCreateStatement { get; set; }

        public int NumberOfRows { get; set; }
    }
}
