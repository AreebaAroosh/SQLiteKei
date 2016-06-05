using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.Generic;
using System.Linq;

namespace SQLiteKei.Helpers
{
    /// <summary>
    /// A class that helps navigating through the tree view.
    /// </summary>
    public static class TreeViewHelper
    {
        /// <summary>
        /// Removes the item from hierarchy.
        /// </summary>
        /// <param name="treeItems">The tree items.</param>
        /// <param name="treeItem">The tree item.</param>
        public static void RemoveItemFromHierarchy(ICollection<TreeItem> treeItems, TreeItem treeItem)
        {
            foreach(var item in treeItems)
            {
                if(item == treeItem)
                {
                    treeItems.Remove(item);
                    break;
                }

                var directory = item as DirectoryItem;

                if(directory != null && directory.Items.Any())
                {
                    RemoveItemFromHierarchy(directory.Items, treeItem);
                }
            }
        }
    }
}
