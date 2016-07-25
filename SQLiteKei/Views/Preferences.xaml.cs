using SQLiteKei.ViewModels.PreferencesWindow;

using System.Windows;

namespace SQLiteKei.Views
{
    /// <summary>
    /// Interaction logic for Preferences.xaml
    /// </summary>
    public partial class Preferences : Window
    {
        public Preferences()
        {
            DataContext = new PreferencesViewModel();
            InitializeComponent();
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
