using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using SQLiteKei.ViewModels.MainTabControl.Tables;
using SQLiteKei.Helpers;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TableGeneralTabContent.xaml
    /// </summary>
    public partial class TableGeneralTabContent : UserControl, INotifyPropertyChanged
    {
        private GeneralTableViewModel tableInfo;

        public GeneralTableViewModel TableInfo
        {
            get { return tableInfo; }
            set
            {
                tableInfo = value;
                if (tableInfo.ColumnData.Any())
                {
                    ColumnDataGrid.Visibility = Visibility.Visible;
                    NoColumnsFoundLabel.Visibility = Visibility.Hidden;
                }
                else
                {
                    NoColumnsFoundLabel.Visibility = Visibility.Visible;
                    ColumnDataGrid.Visibility = Visibility.Hidden;
                }
                NotifyPropertyChanged("TableInfo");
            }
        }

        public TableGeneralTabContent()
        {
            InitializeComponent();
        }

        private void EmptyTable(object sender, RoutedEventArgs e)
        {
            var message = LocalisationHelper.GetString("MessageBox_EmptyTable", tableInfo.TableName);
            var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_EmptyTable");
            var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (result != MessageBoxResult.Yes) return;

            tableInfo.EmptyTable();
        }

        private void ReindexTable(object sender, RoutedEventArgs e)
        {
            var message = LocalisationHelper.GetString("MessageBox_ReindexTable", tableInfo.TableName);
            var messageTitle = LocalisationHelper.GetString("MessageBoxTitle_ReindexTable");
            var result = MessageBox.Show(message, messageTitle, MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result != MessageBoxResult.Yes) return;

            tableInfo.ReindexTable();
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
