using System.Collections.ObjectModel;

namespace SQLiteKei.ViewModels.DBTreeView.Base
{
    public abstract class DirectoryItem : TreeItem
    {
        public ObservableCollection<TreeItem> Items { get; set; }

        public DirectoryItem()
        {
            Items = new ObservableCollection<TreeItem>();
        }
    }
}