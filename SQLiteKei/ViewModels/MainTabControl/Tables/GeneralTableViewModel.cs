using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;

using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    public class GeneralTableViewModel
    {
        public GeneralTableViewModel(string tableName)
        {
            ColumnData = new List<ColumnDataItem>();
            TableName = tableName;

            Initialize();
        }

        public string TableName { get; set; }

        public string TableCreateStatement { get; set; }

        public int ColumnCount { get; set; }

        public double RowCount { get; set; }

        public List<ColumnDataItem> ColumnData { get; set; }

        private void Initialize()
        {
            var dbHandler = new DatabaseHandler(Application.Current.Properties["CurrentDatabase"].ToString());

            TableCreateStatement = dbHandler.GetCreateStatement(TableName);
            RowCount = dbHandler.GetRowCount(TableName);
            var columns = dbHandler.GetColumns(TableName);
            ColumnCount = columns.Count;

            foreach(var column in columns)
            {
                ColumnData.Add(MapToColumnData(column));
            }
        }

        private ColumnDataItem MapToColumnData(Column column)
        {
            return new ColumnDataItem
            {
                Name = column.Name,
                DataType = column.DataType,
                IsNotNullable = column.IsNotNullable,
                IsPrimary = column.IsPrimary,
                DefaultValue = column.DefaultValue
            };
        }
    }
}
