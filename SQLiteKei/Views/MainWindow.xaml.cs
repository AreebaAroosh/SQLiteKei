#region usings

using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;
using SQLiteKei.Views;

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

#endregion

namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private ObservableCollection<TreeItem> treeViewItems;
        public ObservableCollection<TreeItem> TreeViewItems
        {
            get
            {
                if(treeViewItems == null)
                    treeViewItems = new ObservableCollection<TreeItem>();
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

        private void CreateNewDatabase(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    SQLiteConnection.CreateFile(dialog.FileName);
                    AddDatabaseSchemaToTreeView(dialog.FileName);
                }
            }
        }

        private void Button_RemoveDatabase_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as DatabaseItem;

            if (selectedItem != null)
                TreeViewItems.Remove(selectedItem);
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

        private void DBTreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            ResetTabControl();
            var currentSelection = (TreeItem)DBTreeView.SelectedItem;

            var tabs = DatabaseTabGenerator.GenerateTabsFor(currentSelection);

            foreach (TabItem tab in tabs)
                MainTabControl.Items.Add(tab);

            MainTabControl.SelectedIndex = 0;
        }

        private void ResetTabControl()
        {
            var openTabs = MainTabControl.Items.Count;

            for (int i = openTabs-1; i >= 0; i--)
                MainTabControl.Items.RemoveAt(i);
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
