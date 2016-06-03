namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    public class ColumnOverviewDataItem
    {
        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNullable { get; set; }

        public bool IsPrimary { get; set; }
    }
}
