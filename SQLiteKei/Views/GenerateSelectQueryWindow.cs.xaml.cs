#region usings

using SQLiteKei.ViewModels.SelectQueryCreationWindow;

using System.ComponentModel;
using System.Windows;

using SQLiteKei.DataAccess.Database;

#endregion

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for GenerateSelectQueryWindow.xaml
    /// </summary>
    public partial class GenerateSelectQueryWindow : Window, INotifyPropertyChanged
    {
        private SelectQueryCreateViewModel viewModel;
        public SelectQueryCreateViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; NotifyPropertyChanged("ViewModel"); }
        }

        public GenerateSelectQueryWindow(string tableName)
        {
            InitializeComponent();

            ViewModel = GenerateViewModelFromTableName(tableName);
        }

        private SelectQueryCreateViewModel GenerateViewModelFromTableName(string tableName)
        {
            var viewModel = new SelectQueryCreateViewModel(tableName);

            var databasePath = ((App)Application.Current).CurrentDatabase;
            var databaseHandler = new DatabaseHandler(databasePath);

            var columns = databaseHandler.GetColumns(tableName);

            foreach(var column in columns)
            {
                viewModel.Selects.Add(new SelectItem
                {
                    ColumnName = column.Name,
                    IsSelected = true
                });
            }

            return viewModel;
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}
