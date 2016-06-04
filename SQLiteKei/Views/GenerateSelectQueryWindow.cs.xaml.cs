#region usings

using SQLiteKei.ViewModels.SelectQueryCreationWindow;

using System.ComponentModel;
using System.Windows;

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

            ViewModel = new SelectQueryCreateViewModel(tableName);

            ViewModel.Columns.Add(new ColumnSelectItem
            {
                ColumnName = "Name",
                IsSelected = true
            });
            ViewModel.Columns.Add(new ColumnSelectItem
            {
                ColumnName = "Name2",
                IsSelected = true
            });
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
