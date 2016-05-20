#region usings

using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;
using SQLiteKei.Views;

using System.Collections;
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
                if(treeViewItems == null)
                    treeViewItems = new ObservableCollection<TreeViewItem>();
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

        private void Button_RemoveDatabase_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as DatabaseItem;

            if (selectedItem != null)
                TreeViewItems.Remove(selectedItem);
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
                    AddDatabaseSchemaToTreeView(dialog.FileName);
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
                    AddDatabaseSchemaToTreeView(dialog.FileName);
                }
            }
        }

        private void AddDatabaseSchemaToTreeView(string databasePath)
        {
            var schemaMapper = new SchemaToViewModelMapper();
            DatabaseItem databaseItem = schemaMapper.MapSchemaToViewModel(databasePath);

            TreeViewItems.Add(databaseItem);
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
