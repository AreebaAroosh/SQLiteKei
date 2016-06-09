namespace SQLiteKei.ViewModels.MainTabControl.Databases
{
    /// <summary>
    /// A ViewModel that is used in the main tab view to display general table data when a database item is selected.
    /// </summary>
    public class TableOverviewDataItem
    {
        public string Name { get; set; }

        public long RowCount { get; set; }

        public int ColumnCount { get; set; }
    }
}
