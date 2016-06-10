using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.ViewModels.MainTabControl.Tables;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TableRecordsTabContent.xaml
    /// </summary>
    public partial class TableRecordsTabContent : UserControl
    {
        public TableRecordsDataItem TableInfo { get; set; }

        public TableRecordsTabContent()
        {
            InitializeComponent();
        }

        private void ExecuteSelect(object sender, RoutedEventArgs e)
        {
            var createSelectWindow = new GenerateSelectQueryWindow(TableInfo.TableName);

            if(createSelectWindow.ShowDialog().Value)
            {
                StatusBar.Text = string.Empty;
                Execute(createSelectWindow.ViewModel.SelectQuery);
            }
        }

        private void Execute(string selectQuery)
        {
            try
            {
                var dbHandler = new DatabaseHandler(Application.Current.Properties["CurrentDatabase"].ToString());
                var resultTable = dbHandler.ExecuteReader(selectQuery);

                RecordsDataGrid.ItemsSource = resultTable.DefaultView;
                StatusBar.Text = string.Format("Rows returned: {0}", resultTable.Rows.Count);       
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
