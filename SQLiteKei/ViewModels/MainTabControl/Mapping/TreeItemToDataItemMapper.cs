#region usings

using System.Linq;

using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.MainTabControl.Databases;

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
                NumberOfTables = databaseItem.NumberOfTables,
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

                databaseDataItem.NumberOfRecords += tableDataItem.RowCount;
                databaseDataItem.TableOverviewData.Add(tableDataItem);
            }

            return databaseDataItem;
        }
    }
}
