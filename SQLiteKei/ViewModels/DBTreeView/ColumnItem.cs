using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.ViewModels.DBTreeView
{
    public class ColumnItem : TreeItem
    {
        public string DataType { get; set; }

        public bool IsNotNullable { get; set; }

        public bool IsPrimary { get; set; }

        public object DefaultValue { get; set; }
    }
}
