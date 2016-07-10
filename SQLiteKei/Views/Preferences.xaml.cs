using SQLiteKei.ViewModels.PreferencesWindow;

using System.Windows;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        private PreferencesViewModel viewModel;

        public Preferences()
        {
            viewModel = new PreferencesViewModel();
            DataContext = viewModel;
            InitializeComponent();
        }

        private void ApplySettings(object sender, RoutedEventArgs e)
        {
            viewModel.ApplySettings();
            Close();
        }
    }
}
