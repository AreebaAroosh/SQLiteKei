#region usings

using SQLiteKei.ViewModels;
using SQLiteKei.Views;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Forms;

#endregion

namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TreeViewItem> treeViewItems;
        public ObservableCollection<TreeViewItem> TreeViewItems
        {
            get
            {
                return treeViewItems;
            }
            set
            {
                treeViewItems = value;
                NotifyPropertyChanged("TreeViewItems");
            }
        }

        public MainWindow()
        {
            TreeViewItems = new ObservableCollection<TreeViewItem>();
            var directory = new DirectoryItem { Name = "Directory" };
            directory.Items.Add(new TableItem { Name = "Item" });

            TreeViewItems.Add(directory);

            

            InitializeComponent();

            DBTreeView.ItemsSource = TreeViewItems;
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            var window = new About();
            window.ShowDialog();
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

        private void OpenDatabaseFile(object sender, RoutedEventArgs e)
        {
            //using (var dialog = new OpenFileDialog())
            //{
            //    dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
            //    if(dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        AddDatabaseToTreeView(dialog.FileName);
            //    }
            //}
            var directory = new DirectoryItem { Name = "Directory" };
            directory.Items.Add(new TableItem { Name = "Item" });

            TreeViewItems.Add(directory);

        }

        private void AddDatabaseToTreeView(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source=" + databasePath;
                connection.Open();

                DataTable x = connection.GetSchema("Tables");

                foreach(var row in x.Rows)
                {

                }
            }
        }

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
