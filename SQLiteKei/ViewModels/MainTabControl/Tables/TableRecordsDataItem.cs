namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    /// <summary>
    /// A ViewModel that is used in the table records tab.
    /// </summary>
    public class TableRecordsDataItem
    {
        public TableRecordsDataItem(string tableName)
        {
            TableName = tableName;
        }

        public string TableName { get; set; }
    }
}
