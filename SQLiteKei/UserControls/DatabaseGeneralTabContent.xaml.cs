﻿using SQLiteKei.ViewModels.MainTabControl.Databases;

using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SQLiteKei.UserControls
{
    /// <summary>
    /// Interaction logic for DatabaseGeneralTabContent.xaml
    /// </summary>
    public partial class DatabaseGeneralTabContent : UserControl, INotifyPropertyChanged
    {
        private GeneralDatabaseDataItem databaseInfo;

        public GeneralDatabaseDataItem DatabaseInfo
        {
            get { return databaseInfo; }
            set
            {
                databaseInfo = value;
                if (databaseInfo.TableOverviewData.Any())
                {
                    TableDataGrid.Visibility = Visibility.Visible;
                    NoTablesFoundLabel.Visibility = Visibility.Hidden;
                }
                else
                {
                    NoTablesFoundLabel.Visibility = Visibility.Visible;
                    TableDataGrid.Visibility = Visibility.Hidden;
                }
                NotifyPropertyChanged("DatabaseInfo");            }
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
