#region usings

using SQLiteKei.Views;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Forms;

#endregion

namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Button_NewDatabase_Click(object sender, RoutedEventArgs e)
        {
            CreateDatabaseFromFileDialog();
        }

        private void CreateDatabaseFromFileDialog()
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SQLiteConnection.CreateFile(dialog.FileName);
                    //TODO: remove the message box when the newly generated database is shown and selected in main tree view automatically
                    System.Windows.MessageBox.Show("Database created successfully.", "DB Creation Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            var window = new About();
            window.ShowDialog();
        }
    }
}
