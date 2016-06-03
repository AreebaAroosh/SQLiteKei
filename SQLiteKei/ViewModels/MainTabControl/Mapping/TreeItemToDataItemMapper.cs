#region usings

using System.Linq;

using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.MainTabControl.Databases;
using SQLiteKei.ViewModels.MainTabControl.Tables;
using System.Collections.Generic;
using System;

#endregion

namespace SQLiteKei.ViewModels.MainTabControl.Mapping
{
    public class TreeItemToDataItemMapper
    {
        //TODO think about property error/exception handling on this one
        public GeneralDatabaseDataItem MapToGeneralDatabaseDataItem(DatabaseItem databaseItem)
        {
            var databaseDataItem = new GeneralDatabaseDataItem
            {
                DisplayName = databaseItem.DisplayName,
                Name = databaseItem.Name,
                FilePath = databaseItem.FilePath,
                TableCount = databaseItem.NumberOfTables,
            };

            var tableItems = databaseItem.Items.Single(x => x.DisplayName == "Tables") as TableFolderItem;

            foreach (TableItem table in tableItems.Items)
            {
                var tableDataItem = new TableOverviewDataItem
                {
                    Name = table.DisplayName,
                    ColumnCount = table.ColumnCount,
                    RowCount = table.RowCount
                };

                databaseDataItem.RowCount += tableDataItem.RowCount;
                databaseDataItem.TableOverviewData.Add(tableDataItem);
            }

            return databaseDataItem;
        }

        public GeneralTableDataItem MapToGeneralTableDataItem(TableItem tableItem)
        {
            var tableDataItem = new GeneralTableDataItem
            {
                Name = tableItem.DisplayName,
                ColumnCount = tableItem.ColumnCount,
                RowCount = tableItem.RowCount,
                TableCreateStatement = tableItem.TableCreateStatement
            };

            List<ColumnOverviewDataItem> columnDataItems = MapColumns(tableItem.Columns);

            foreach (var columnDataItem in columnDataItems)
                tableDataItem.ColumnData.Add(columnDataItem);

            return tableDataItem;
        }

        private List<ColumnOverviewDataItem> MapColumns(List<ColumnItem> columns)
        {
            var columnDataItems = new List<ColumnOverviewDataItem>();

            foreach (ColumnItem column in columns)
            {
                columnDataItems.Add(new ColumnOverviewDataItem
                {
                    Name = column.DisplayName,
                    DataType = column.DataType,
                    IsNotNullable = column.IsNotNullable,
                    IsPrimary = column.IsPrimary,
                    DefaultValue = column.DefaultValue
                });
            }
            return columnDataItems;
        }

        public TableRecordsDataItem MapToTableRecordsDataItem(TableItem tableItem)
        {
            var recordsDataItem = new TableRecordsDataItem
            {
                TableName = tableItem.DisplayName,
                DatabasePath = tableItem.DatabasePath
            };

            List<ColumnOverviewDataItem> columnDataItems = MapColumns(tableItem.Columns);

            foreach (var columnDataItem in columnDataItems)
                recordsDataItem.Columns.Add(columnDataItem);

            return recordsDataItem;
        }
    }
}
