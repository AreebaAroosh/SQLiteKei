using SQLiteKei.ViewModels.Common;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.TableCreatorWindow;

using System.Collections.Generic;
using System.Windows;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for TableCreator.xaml
    /// </summary>
    public partial class TableCreator : Window
    {
        public TableCreator(IEnumerable<TreeItem> databases)
        {
            var viewModel = new TableCreatorViewModel();

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
    }
}
