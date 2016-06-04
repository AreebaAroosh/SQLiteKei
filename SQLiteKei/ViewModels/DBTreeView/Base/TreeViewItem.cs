namespace SQLiteKei.ViewModels.DBTreeView.Base
{
    public abstract class TreeItem
    {
        public string DisplayName { get; set; }

        public string DatabasePath { get; set; }
    }
}