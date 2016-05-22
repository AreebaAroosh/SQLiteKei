using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.ViewModels.DBTreeView
{
    public class DatabaseItem : DirectoryItem
    {
        public string FilePath { get; set; }

        public int NumberOfTables { get; set; }
    }
}
