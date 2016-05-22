#region usings

using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;

using System.Collections.Generic;
using System.Windows.Controls;
using System;
using SQLiteKei.UserControls;

#endregion

namespace SQLiteKei.Helpers
{
    static class DatabaseTabGenerator
    {
        public static List<TabItem> GenerateTabsFor(TreeItem treeItem)
        {
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
            throw new NotImplementedException();
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
