#region usings

using SQLiteKei.UserControls;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.Generic;
using System.Windows.Controls;
using System;


#endregion

namespace SQLiteKei.Helpers
{
    /// <summary>
    /// A class that generates tabs for the currently selected tree item in the main tree.
    /// </summary>
    static class DatabaseTabGenerator
    {
        /// <summary>
        /// Generates the tabs for the specified tree item depending on its type.
        /// </summary>
        /// <param name="treeItem">The tree item.</param>
        /// <returns></returns>
        public static List<TabItem> GenerateTabsFor(TreeItem treeItem)
        {
            if (treeItem == null)
                return DefaultTabs();

            if (treeItem.GetType() == typeof(DatabaseItem))
                return DatabaseTabs(treeItem);
            else if (treeItem.GetType() == typeof(TableItem))
                return TableTabs(treeItem);
            else if (treeItem.GetType() == typeof(IndexItem))
                return IndexTabs(treeItem);
            else
                return DefaultTabs();
        }

        private static List<TabItem> DefaultTabs()
        {
            return new List<TabItem>();
            //throw new NotImplementedException();
        }

        private static List<TabItem> IndexTabs(TreeItem treeItem)
        {
            throw new NotImplementedException();
        }

        private static List<TabItem> TableTabs(TreeItem treeItem)
        {
            throw new NotImplementedException();
        }

        private static List<TabItem> DatabaseTabs(TreeItem treeItem)
        {
            var databaseItem = treeItem as DatabaseItem;

            var tabs = new List<TabItem>();

            TabItem generalTab = new TabItem() { Header = "General" };
            generalTab.Content = new DatabaseGeneralTabContent();

            tabs.Add(generalTab);

            return tabs;
        }
    }
}
