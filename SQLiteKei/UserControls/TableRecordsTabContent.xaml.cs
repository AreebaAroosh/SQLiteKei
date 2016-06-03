#region usings

using SQLiteKei.ViewModels.MainTabControl.Tables;

using System.Windows.Controls;
using System;

#endregion

namespace SQLiteKei.UserControls
{
    /// <summary>
    /// Interaction logic for TableRecordsTabContent.xaml
    /// </summary>
    public partial class TableRecordsTabContent : UserControl
    {
        private TableRecordsDataItem TableInfo { get; set; }

        public TableRecordsTabContent(TableRecordsDataItem tableInfo)
        {
            InitializeComponent();

            TableInfo = tableInfo;
            SetTableColumns();
        }

        private void SetTableColumns()
        {
            foreach(var column in TableInfo.Columns)
            {
                RecordsDataGrid.Columns.Add(new DataGridTextColumn
                {
                    Header = column.Name
                });
            }
        }
    }
}
