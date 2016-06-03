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
                return GenerateDefaultTabs();

            if (treeItem.GetType() == typeof(DatabaseItem))
                return GenerateDatabaseTabs((DatabaseItem)treeItem);
            if (treeItem.GetType() == typeof(TableItem))
                return GenerateTableTabs((TableItem)treeItem);
            if (treeItem.GetType() == typeof(IndexItem))
                return GenerateIndexTabs((IndexItem)treeItem);
            return GenerateDefaultTabs();
        }

        private static List<TabItem> GenerateDefaultTabs()
        {
            return new List<TabItem>();
            //throw new NotImplementedException();
        }

        private static List<TabItem> GenerateIndexTabs(IndexItem indexItem)
        {
            throw new NotImplementedException();
        }

        private static List<TabItem> GenerateTableTabs(TableItem tableItem)
        {
            var mapper = new TreeItemToDataItemMapper();

            var generalTab = new TabItem
            {
                Header = string.Format("Table {0}", tableItem.DisplayName),
                Content = new TableGeneralTabContent
                {
                    TableInfo = mapper.MapToGeneralTableDataItem(tableItem)
                }
            };

            return new List<TabItem> { generalTab };
        }

        private static List<TabItem> GenerateDatabaseTabs(DatabaseItem databaseItem)
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
