#region usings

using SQLiteKei.ViewModels.MainTabControl.Tables;

using System.Windows.Controls;
using System;
using System.Data.Common;
using System.Data;
using SQLiteKei.Queries.Builders;
using System.Text.RegularExpressions;
using SQLiteKei.Views;

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
        }

        private void ExecuteSelect(object sender, System.Windows.RoutedEventArgs e)
        {
            var createSelectWindow = new GenerateSelectQueryWindow(TableInfo.TableName);

            if(createSelectWindow.ShowDialog().Value == true)
            {
                StatusBar.Text = string.Empty;
                Execute(createSelectWindow.ViewModel.SelectQuery);
            }
        }

        private void Execute(string selectQuery)
        {
            try
            {
                var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
                using (var connection = factory.CreateConnection())
                {
                    connection.ConnectionString = string.Format("Data Source={0}", TableInfo.DatabasePath);
                    connection.Open();

                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = selectQuery;

                        var resultTable = new DataTable();
                        resultTable.Load(command.ExecuteReader());

                        RecordsDataGrid.ItemsSource = resultTable.DefaultView;

                        StatusBar.Text = string.Format("Rows returned: {0}", resultTable.Rows.Count);
                    }
                }
            }
            catch (Exception ex)
            {
                var oneLineMessage = Regex.Replace(ex.Message, @"\n", " ");
                oneLineMessage = Regex.Replace(oneLineMessage, @"\t|\r", "");
                oneLineMessage = oneLineMessage.Replace("SQL logic error or missing database ", "SQL logic error or missing database - ");

                StatusBar.Text = oneLineMessage;
            }
        }
    }
}
