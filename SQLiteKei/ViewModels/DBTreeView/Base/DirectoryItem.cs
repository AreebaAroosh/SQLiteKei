using System.Collections.Generic;

namespace SQLiteKei.ViewModels.DBTreeView.Base
{
    public abstract class DirectoryItem : TreeItem
    {
        public List<TreeItem> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<TreeItem>();
        }
    }
}