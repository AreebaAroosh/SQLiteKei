using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.QueryEditorWindow;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;

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

            KeyDown += new KeyEventHandler(Window_KeyDown);

            InitializeComponent();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return && (Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control)
                Execute();
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            Execute();
        }

        private void Execute()
        {
            viewModel.StatusInfo = string.Empty;

            if (DatabaseComboBox.SelectedItem == null)
            {
                viewModel.StatusInfo = LocalisationHelper.GetString("TableCreator_NoDatabaseSelected");
                return;
            }

            if (string.IsNullOrEmpty(viewModel.SqlStatement)) return;

            if (string.IsNullOrEmpty(QueryBox.SelectedText))
            {
                ExecuteSql(viewModel.SqlStatement);
            }
            else
            {
                ExecuteSql(QueryBox.SelectedText);    
            }
        }

        private void ExecuteSql(string sqlStatement)
        {
            var database = DatabaseComboBox.SelectedItem as DatabaseSelectItem;
            var dbHandler = new DatabaseHandler(database.DatabasePath);

            try
            {
                if (viewModel.SqlStatement.StartsWith("SELECT", StringComparison.CurrentCultureIgnoreCase))
                {
                    var queryResult = dbHandler.ExecuteReader(sqlStatement);

                    QueryGrid.ItemsSource = queryResult.DefaultView;
                    viewModel.StatusInfo = string.Format("Rows returned: {0}", queryResult.Rows.Count);
                }
                else
                {
                    var commandResult = dbHandler.ExecuteNonQuery(sqlStatement);

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
