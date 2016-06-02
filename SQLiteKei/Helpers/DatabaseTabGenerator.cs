#region usings

using SQLiteKei.UserControls;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainTabControl.Mapping;

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
                return DatabaseTabs((DatabaseItem)treeItem);
            if (treeItem.GetType() == typeof(TableItem))
                return TableTabs((TableItem)treeItem);
            if (treeItem.GetType() == typeof(IndexItem))
                return IndexTabs((IndexItem)treeItem);
            return DefaultTabs();
        }

        private static List<TabItem> DefaultTabs()
        {
            return new List<TabItem>();
            //throw new NotImplementedException();
        }

        private static List<TabItem> IndexTabs(IndexItem indexItem)
        {
            throw new NotImplementedException();
        }

        private static List<TabItem> TableTabs(TableItem tableItem)
        {
            //TODO var mapper = new TreeItemToDataItemMapper();

            var generalTab = new TabItem
            {
                Header = string.Format("Table {0}", tableItem.DisplayName),
                Content = new TableGeneralTabContent()
            };

            return new List<TabItem> { generalTab };
        }

        private static List<TabItem> DatabaseTabs(DatabaseItem databaseItem)
        {
            var mapper = new TreeItemToDataItemMapper();

            var generalTab = new TabItem
            {
                Header = databaseItem.DisplayName,
                Content = new DatabaseGeneralTabContent
                {
                    DatabaseInfo = mapper.MapToGeneralDatabaseDataItem(databaseItem) 
                }
            };

            return new List<TabItem> { generalTab };
        }
    }
}
