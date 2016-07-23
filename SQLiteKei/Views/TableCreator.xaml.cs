using log4net;

using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.TableCreatorWindow;

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for TableCreator.xaml
    /// </summary>
    public partial class TableCreator : Window
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private readonly TableCreatorViewModel viewModel;

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

            DataContext = viewModel;
            InitializeComponent();
        }

        private void Create(object sender, RoutedEventArgs e)
        {
            viewModel.StatusInfo = string.Empty;

            if (DatabaseComboBox.SelectedItem == null)
            {
                viewModel.StatusInfo = LocalisationHelper.GetString("TableCreator_NoDatabaseSelected");
                return;
            } 

            if (!string.IsNullOrEmpty(viewModel.SqlStatement))
            {
                var database = DatabaseComboBox.SelectedItem as DatabaseSelectItem;
                var dbHandler = new DatabaseHandler(database.DatabasePath);

                try
                {
                    if (viewModel.SqlStatement.StartsWith("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbHandler.ExecuteNonQuery(viewModel.SqlStatement);
                        viewModel.StatusInfo = LocalisationHelper.GetString("TableCreator_TableCreateSuccess");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("An error occured when the user tried to create a table from the TableCreator.", ex);
                    viewModel.StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }

        private void AddColumn(object sender, RoutedEventArgs e)
        {
            viewModel.AddColumnDefinition();
        }

        private void AddForeignKey(object sender, RoutedEventArgs e)
        {
            viewModel.AddForeignKeyDefinition();
        }
    }
}
