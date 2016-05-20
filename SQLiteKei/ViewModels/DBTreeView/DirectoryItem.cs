using System.Collections.Generic;

namespace SQLiteKei.ViewModels.DBTreeView
{
    public class DirectoryItem : TreeViewItem
    {
        public List<TreeViewItem> Items { get; set; }

        public DirectoryItem()
        {
            Items = new List<TreeViewItem>();
        }
    }
}