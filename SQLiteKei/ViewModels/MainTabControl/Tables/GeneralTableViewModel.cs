using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.Models;
using SQLiteKei.ViewModels.Base;

using System.Collections.Generic;

namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    public class GeneralTableViewModel : NotifyingModel
    {
        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    try
                    {
                        using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
                        {
                            tableHandler.RenameTable(tableName, value);
                        }
                        tableName = value;
                        NotifyPropertyChanged("TableName");
                    }
                    catch 
                    { 
                        //TODO decide what should happen in this case 
                    }
                }
            }
        }

        private long rowCount;
        public long RowCount
        {
            get { return rowCount; }
            set { rowCount = value; NotifyPropertyChanged("RowCount"); }
        }

        public int ColumnCount { get; set; }

        public string TableCreateStatement { get; set; }

        public List<ColumnDataItem> ColumnData { get; set; }

        public GeneralTableViewModel(string tableName)
        {
            ColumnData = new List<ColumnDataItem>();
            this.tableName = tableName;

            Initialize();
        }

        private void Initialize()
        {
            using (var dbHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                TableCreateStatement = dbHandler.GetCreateStatement(TableName);
                RowCount = dbHandler.GetRowCount(TableName);
                var columns = dbHandler.GetColumns(TableName);
                ColumnCount = columns.Count;

                foreach (var column in columns)
                {
                    ColumnData.Add(MapToColumnData(column));
                }
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

        internal void EmptyTable()
        {
            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                tableHandler.EmptyTable(TableName);
                RowCount = 0;
            }
        }

        internal void ReindexTable()
        {
            using (var tableHandler = new TableHandler(Properties.Settings.Default.CurrentDatabase))
            {
                tableHandler.ReindexTable(TableName);
            }
        }
    }
}
