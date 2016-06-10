using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainTabControl.Databases;
using SQLiteKei.ViewModels.MainTabControl.Tables;

using System.Collections.Generic;
using System.Windows.Controls;
using System;
using SQLiteKei.Views.UserControls;


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
        }

        private static List<TabItem> GenerateDatabaseTabs(DatabaseItem databaseItem)
        {
            var generalTab = new TabItem
            {
                Header = databaseItem.DisplayName,
                Content = new DatabaseGeneralTabContent
                {
                    DatabaseInfo = new GeneralDatabaseViewModel(databaseItem.DatabasePath)
                }
            };

            return new List<TabItem> { generalTab };
        }

        private static List<TabItem> GenerateTableTabs(TableItem tableItem)
        {
            var generalTab = new TabItem
            {
                Header = string.Format("Table {0}", tableItem.DisplayName),
                Content = new TableGeneralTabContent
                {
                    TableInfo = new GeneralTableDataItem(tableItem.DisplayName)
                }
            };

            var recordsTab = new TabItem
            {
                Header = "Records",
                Content = new TableRecordsTabContent
                {
                    TableInfo = new  TableRecordsDataItem(tableItem.DisplayName)
                }
            };

            return new List<TabItem> { generalTab, recordsTab };
        }

        private static List<TabItem> GenerateIndexTabs(IndexItem indexItem)
        {
            throw new NotImplementedException();
        }


    }
}
