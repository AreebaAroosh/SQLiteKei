using System.ComponentModel;
using SQLiteKei.ViewModels.DBTreeView;
using System.Windows.Controls;

namespace SQLiteKei.UserControls
{
    /// <summary>
    /// Interaction logic for DatabaseGeneralTabContent.xaml
    /// </summary>
    public partial class DatabaseGeneralTabContent : UserControl, INotifyPropertyChanged
    {
        private DatabaseItem databaseInfo;

        public DatabaseItem DatabaseInfo
        {
            get { return databaseInfo; }
            set { databaseInfo = value; NotifyPropertyChanged("DatabaseInfo"); }
        }

        public DatabaseGeneralTabContent()
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
