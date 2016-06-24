using SQLiteKei.ViewModels.SelectQueryWindow;

using System.Windows;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for GenerateSelectQueryWindow.xaml
    /// </summary>
    public partial class SelectQueryWindow : Window
    {
        public SelectQueryWindow(string tableName)
        {
            InitializeComponent();
            DataContext = new SelectQueryViewModel(tableName);
        }

        private void Execute(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void AddOrderStatement(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as SelectQueryViewModel;
            viewModel.AddOrderStatement();
        }
    }
}
