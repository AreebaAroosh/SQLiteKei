#region usings

using SQLiteKei.ViewModels.SelectQueryWindow;

using System.ComponentModel;
using System.Windows;

#endregion

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for GenerateSelectQueryWindow.xaml
    /// </summary>
    public partial class SelectQueryWindow : Window, INotifyPropertyChanged
    {
        private SelectQueryCreateViewModel viewModel;
        public SelectQueryCreateViewModel ViewModel
        {
            get { return viewModel; }
            set { viewModel = value; NotifyPropertyChanged("ViewModel"); }
        }

        public SelectQueryWindow(string tableName)
        {
            InitializeComponent();
            ViewModel = new SelectQueryCreateViewModel(tableName);
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
