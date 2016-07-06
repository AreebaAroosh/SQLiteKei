using SQLiteKei.DataAccess.Database;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.DBTreeView;
using SQLiteKei.ViewModels.DBTreeView.Base;
using SQLiteKei.ViewModels.MainWindow;
using SQLiteKei.Views;

using System;
using System.IO;
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
    public partial class MainWindow
    {
        private readonly MainWindowViewModel viewModel;

        public MainWindow()
        {
            viewModel = new MainWindowViewModel(new TreeSaveHelper());
            DataContext = viewModel;
            InitializeComponent();
        }

        private void MenuItem_Exit_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void OpenAbout(object sender, RoutedEventArgs e)
        {
            new About().ShowDialog();
        }

        private void OpenPreferences(object sender, RoutedEventArgs e)
        {
            new Preferences().ShowDialog();
        }

        private void OpenQueryEditor(object sender, RoutedEventArgs e)
        {
            new QueryEditor(viewModel.TreeViewItems).ShowDialog();
        }

        private void OpenTableCreator(object sender, RoutedEventArgs e)
        {
            new TableCreator(viewModel.TreeViewItems).ShowDialog();
            viewModel.RefreshTree();
        }

        private void CreateNewDatabase(object sender, RoutedEventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "SQLite (*.sqlite)|*.sqlite";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    DatabaseHandler.CreateDatabase(dialog.FileName);
                    viewModel.OpenDatabase(dialog.FileName);
                }
            }
        }

        private void CloseDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as TreeItem;

            if (selectedItem != null)
                viewModel.CloseDatabase(selectedItem.DatabasePath);
        }

        private void OpenDatabase(object sender, RoutedEventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Database Files (*.sqlite, *.db)|*.sqlite; *db; |All Files |*.*";
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    viewModel.OpenDatabase(dialog.FileName);
                }
            }
        }

        private void DeleteDatabase(object sender, RoutedEventArgs e)
        {
            var selectedItem = DBTreeView.SelectedItem as DatabaseItem;

            if (selectedItem != null)
            {
                var message = LocalisationHelper.GetString("MessageBox_DatabaseDeleteWarning", selectedItem.DisplayName);
                var result = System.Windows.MessageBox.Show(message, LocalisationHelper.GetString("MessageBoxTitle_DatabaseDeletion"), MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result != MessageBoxResult.Yes) return;
                if (!File.Exists(selectedItem.DatabasePath))
                    throw new FileNotFoundException("Database file could not be found.");

                File.Delete(selectedItem.DatabasePath);
                viewModel.CloseDatabase(selectedItem.DatabasePath);
            }
        }

        private void RefreshTree(object sender, RoutedEventArgs e)
        {
            viewModel.RefreshTree();
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
                Properties.Settings.Default.CurrentDatabase = currentSelection.DatabasePath;
            }
        }

        private void ResetTabControl()
        {
            var openTabs = MainTabControl.Items.Count;

            for (var i = openTabs-1; i >= 0; i--)
                MainTabControl.Items.RemoveAt(i);

            var defaultTabs = DatabaseTabGenerator.GenerateDefaultTabs();

            foreach (TabItem tab in defaultTabs)
                MainTabControl.Items.Add(tab);

            MainTabControl.SelectedIndex = 0;
        }

        private void DeleteTable(object sender, RoutedEventArgs e)
        {
            var tableItem = (TableItem)DBTreeView.SelectedItem;
            viewModel.DeleteTable(tableItem);
        }

        private void EmptyTable(object sender, RoutedEventArgs e)
        {
            var tableItem = (TableItem)DBTreeView.SelectedItem;
            viewModel.EmptyTable(tableItem.DisplayName);
        }

        protected override void OnClosed(EventArgs e)
        {
            viewModel.SaveTree();
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
    }
}
