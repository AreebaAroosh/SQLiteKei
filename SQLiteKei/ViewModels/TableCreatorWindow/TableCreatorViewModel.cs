using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System.Collections.Generic;

namespace SQLiteKei.ViewModels.TableCreatorWindow
{
    public class TableCreatorViewModel : NotifyingItem
    {
        public List<DatabaseSelectItem> Databases { get; set; }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public TableCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            sqlStatement = string.Empty;
        }
    }
}
