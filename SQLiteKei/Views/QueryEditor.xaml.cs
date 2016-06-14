using SQLiteKei.DataAccess.Database;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.QueryEditorWindow;

using System;
using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for QueryEditor.xaml
    /// </summary>
    public partial class QueryEditor : Window
    {
        private QueryEditorViewModel viewModel;

        public QueryEditor(IEnumerable<TreeItem> databases)
        {
            viewModel = new QueryEditorViewModel();

            foreach(DatabaseItem database in databases)
            {
                viewModel.Databases.Add(new DatabaseSelectItem
                {
                    DatabaseName = database.DisplayName,
                    DatabasePath = database.DatabasePath
                });
            }

            DataContext = viewModel;
            InitializeComponent();
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            viewModel.StatusInfo = string.Empty;

            if (!string.IsNullOrEmpty(viewModel.SqlStatement))
            {
                var database = DatabaseComboBox.SelectedItem as DatabaseSelectItem;
                var dbHandler = new DatabaseHandler(database.DatabasePath);

                try
                {
                    if (viewModel.SqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var queryResult = dbHandler.ExecuteReader(viewModel.SqlStatement);

                        QueryGrid.ItemsSource = queryResult.DefaultView;
                        viewModel.StatusInfo = string.Format("Rows returned: {0}", queryResult.Rows.Count);
                    }
                    else
                    {
                        var commandResult = dbHandler.ExecuteNonQuery(viewModel.SqlStatement);

                        viewModel.StatusInfo = string.Format("Rows affected: {0}", commandResult);
                    }
                }
                catch (Exception ex)
                {
                    viewModel.StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
                
        }
    }
}
