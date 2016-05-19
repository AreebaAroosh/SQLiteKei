using System.Collections.Generic;

namespace SQLiteKei.ViewModels
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