using SQLiteKei.ViewModels.DBTreeView.Base;

namespace SQLiteKei.ViewModels.DBTreeView
{
    public class DatabaseItem : DirectoryItem
    {
        /// <summary>
        /// The database name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        public string FilePath { get; set; }

        public int NumberOfTables { get; set; }
    }
}
