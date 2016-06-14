using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.DBTreeView.Mapping;
using SQLiteKei.Views;

using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : INotifyPropertyChanged
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
            InitializeTreeView();
            InitializeComponent();
        }

        private void InitializeTreeView()
        {
            var tree = TreeSaveHelper.LoadTree();
            var itemsToRemove = tree.Where(rootItem => !File.Exists(rootItem.DatabasePath));

            foreach (var item in itemsToRemove)
            {
                tree.Remove(item);
            }

            TreeViewItems = tree;
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void MenuItem_About_Click(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
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

        private void CloseDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as DatabaseItem;

            if (selectedItem != null)
                TreeViewItems.Remove(selectedItem);
        }

        private void OpenDatabase(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    if(!TreeViewItems.Any(x => x.DatabasePath.Equals(dialog.FileName)))
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
            SetGlobalDatabaseString();
            ResetTabControl();

            var currentSelection = (TreeItem)DBTreeView.SelectedItem;
            var tabs = DatabaseTabGenerator.GenerateTabsFor(currentSelection);

            foreach (TabItem tab in tabs)
                MainTabControl.Items.Add(tab);
        }

        private void SetGlobalDatabaseString()
        {
            if(DBTreeView.SelectedItem != null)
            {
                var currentSelection = (TreeItem)DBTreeView.SelectedItem;
                (System.Windows.Application.Current as App).CurrentDatabase = currentSelection.DatabasePath;
            }
        }

        private void DeleteDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as DatabaseItem;

            if(selectedItem != null)
            {
                var message = "Do you really want to delete this database? Warning: this will delete the database from your file system.";
                var result = System.Windows.MessageBox.Show(message, "Database Deletion", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    if (!File.Exists(selectedItem.DatabasePath))
                        throw new FileNotFoundException("Database file could not be found.");

                    File.Delete(selectedItem.DatabasePath);
                    TreeViewItems.Remove(selectedItem);
                }
            }
        }

        private void ResetTabControl()
        {
            var openTabs = MainTabControl.Items.Count;

            for (int i = openTabs-1; i >= 0; i--)
                MainTabControl.Items.RemoveAt(i);

            var defaultTabs = DatabaseTabGenerator.GenerateTabsFor(null);

            foreach (TabItem tab in defaultTabs)
                MainTabControl.Items.Add(tab);

            MainTabControl.SelectedIndex = 0;
        }

        private void DeleteTable(object sender, RoutedEventArgs e)
        {
            var tableItem = DBTreeView.SelectedItem as TableItem;

            if(tableItem != null)
            {
                

                var message = string.Format("Do you really want to delete the table '{0}' permanently?", tableItem.DisplayName);
                var result = System.Windows.MessageBox.Show(message, "Delete Table", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if(result == MessageBoxResult.Yes)
                {
                    try
                    {
                        var databasePath = ((App)System.Windows.Application.Current).CurrentDatabase;
                        using (var databaseHandler = new TableHandler(databasePath))
                        {
                            databaseHandler.DropTable(tableItem.DisplayName);
                            ResetTabControl();
                            TreeViewHelper.RemoveItemFromHierarchy(TreeViewItems, tableItem);
                        }
                    }
                    catch (Exception ex)
                    {
                        var statusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                        StatusBarInfo.Text = statusInfo;
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            TreeSaveHelper.SaveTree(TreeViewItems);
        }

        #region TreeViewRightClickEvent
        /// <summary>
        /// Method that is used to make sure a tree view element is selected on a right click event before the context menu is opened.
        /// </summary>
        private void TreeViewRightMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            TreeViewItem treeViewItem = VisualUpwardSearch(e.OriginalSource as DependencyObject);

            if (treeViewItem != null)
            {
                treeViewItem.Focus();
                e.Handled = true;
            }
        }

        static TreeViewItem VisualUpwardSearch(DependencyObject source)
        {
            while (source != null && !(source is TreeViewItem))
                source = VisualTreeHelper.GetParent(source);

            return source as TreeViewItem;
        }
        #endregion

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
