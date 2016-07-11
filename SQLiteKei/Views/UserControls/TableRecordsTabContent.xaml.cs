using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.MainTabControl.Tables;
using SQLiteKei.ViewModels.SelectQueryWindow;

using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TableRecordsTabContent.xaml
    /// </summary>
    public partial class TableRecordsTabContent : UserControl
    {
        private readonly ILog log = LogHelper.GetLogger();

        public TableRecordsDataItem TableInfo { get; set; }

        public RecordsTabViewModel ViewModel { get; set; }

        public TableRecordsTabContent()
        {
            ViewModel = new RecordsTabViewModel();
            DataContext = ViewModel;
            InitializeComponent();
        }

        private void ExecuteSelect(object sender, RoutedEventArgs e)
        {
            var createSelectWindow = new SelectQueryWindow(TableInfo.TableName);

            if(createSelectWindow.ShowDialog().Value)
            {
                StatusBar.Text = string.Empty;
                var windowViewModel = createSelectWindow.DataContext as SelectQueryViewModel;
                Execute(windowViewModel.SelectQuery);
            }
        }

        private void Execute(string selectQuery)
        {
            log.Info("Executing select query from SelectQuery window.\n" + selectQuery);
            try
            {
                var dbPath = Properties.Settings.Default.CurrentDatabase;
                var dbHandler = new DatabaseHandler(dbPath);
                var resultTable = dbHandler.ExecuteReader(selectQuery);

                ViewModel.DataGridCollection = new ListCollectionView(resultTable.DefaultView);
                StatusBar.Text = string.Format("Rows returned: {0}", resultTable.Rows.Count);
            }
            catch (Exception ex)
            {
                log.Error("Select query failed.", ex);
                var oneLineMessage = Regex.Replace(ex.Message, @"\n", " ");
                oneLineMessage = Regex.Replace(oneLineMessage, @"\t|\r", "");
                oneLineMessage = oneLineMessage.Replace("SQL logic error or missing database ", "SQL Error - ");

                StatusBar.Text = oneLineMessage;
            }
        }
    }
}
