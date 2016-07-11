﻿using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainTabControl.Databases;
using SQLiteKei.ViewModels.MainTabControl.Tables;
using SQLiteKei.Views.UserControls;

using System.Collections.Generic;
using System.Windows.Controls;
using System;



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
            return GenerateDefaultTabs();
        }

        public static List<TabItem> GenerateDefaultTabs()
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
                Header = LocalisationHelper.GetString("TabHeader_GeneralTable", tableItem.DisplayName),
                Content = new TableGeneralTabContent
                {
                    TableInfo = new GeneralTableViewModel(tableItem.DisplayName)
                }
            };

            var recordsTab = new TabItem
            {
                Header = LocalisationHelper.GetString("TabHeader_TableRecords"),
                Content = new TableRecordsTabContent
                {
                    TableInfo = new TableRecordsDataItem(tableItem.DisplayName)
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
