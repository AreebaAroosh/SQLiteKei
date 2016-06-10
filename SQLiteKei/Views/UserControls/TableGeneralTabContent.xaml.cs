using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using SQLiteKei.ViewModels.MainTabControl.Tables;

namespace SQLiteKei.Views.UserControls
{
    /// <summary>
    /// Interaction logic for TableGeneralTabContent.xaml
    /// </summary>
    public partial class TableGeneralTabContent : UserControl, INotifyPropertyChanged
    {
        private GeneralTableDataItem tableInfo;

        public GeneralTableDataItem TableInfo
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
