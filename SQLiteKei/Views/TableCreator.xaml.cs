using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders.Enums;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.TableCreatorWindow;

using System;
using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for TableCreator.xaml
    /// </summary>
    public partial class TableCreator : Window
    {
        private TableCreatorViewModel viewModel;

        public TableCreator(IEnumerable<TreeItem> databases)
        {
            viewModel = new TableCreatorViewModel();

            foreach (DatabaseItem database in databases)
            {
                viewModel.Databases.Add(new DatabaseSelectItem
                {
                    DatabaseName = database.DisplayName,
                    DatabasePath = database.DatabasePath
                });
            }

            viewModel.ColumnDefinitions.Add(new ColumnCreateItem
            {
                ColumnName = "Column",
                DataType = DataType.Blob,
                IsNotNull = true,
                IsPrimary = true,
                DefaultValue = "abcde"
            });

            DataContext = viewModel;
            InitializeComponent();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            viewModel.StatusInfo = string.Empty;

            if (!string.IsNullOrEmpty(viewModel.SqlStatement))
            {
                var database = DatabaseComboBox.SelectedItem as DatabaseSelectItem;
                var dbHandler = new DatabaseHandler(database.DatabasePath);

                try
                {
                    if (viewModel.SqlStatement.StartsWith("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        var queryResult = dbHandler.ExecuteNonQuery(viewModel.SqlStatement);
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
