#region usings

using SQLiteKei.ViewModels;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.Views;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
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
            InitializeComponent();
            System.Windows.Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
        }

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
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    AddDatabaseToTreeView(dialog.FileName);
                }
            }
        }

        private void AddDatabaseToTreeView(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            using (var connection = factory.CreateConnection())
            {
                connection.ConnectionString = "Data Source=" + databasePath;
                connection.Open();

                DirectoryItem databaseFileTreeItem = new DirectoryItem()
                {
                    Name = Path.GetFileNameWithoutExtension(databasePath)
                };

                var tables = connection.GetSchema("Tables").AsEnumerable();

                IEnumerable tableNames = tables.Select(x => x.ItemArray[2]);

                foreach(string tableName in tableNames)
                {

                }
            }
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
