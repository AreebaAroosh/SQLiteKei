using SQLiteKei.DataAccess.QueryBuilders.Enums;
using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.TableCreatorWindow
{
    public class ColumnCreateItem : NotifyingItem
    {
        private string columnName;
        public string ColumnName
        {
            get { return columnName; }
            set { columnName = value; NotifyPropertyChanged("ColumnName"); }
        }

        private DataType dataType;
        public DataType DataType
        {
            get { return dataType; }
            set { dataType = value; NotifyPropertyChanged("DataType"); }
        }

        private bool isPrimary;
        public bool IsPrimary
        {
            get { return isPrimary; }
            set { isPrimary = value; NotifyPropertyChanged("IsPrimary"); }
        }

        private bool isNotNull;
        public bool IsNotNull
        {
            get { return isNotNull; }
            set { isNotNull = value; NotifyPropertyChanged("IsNotNull"); }
        }

        private object defaultValue;
        public object DefaultValue
        {
            get { return defaultValue; }
            set { defaultValue = value; NotifyPropertyChanged("DefaultValue"); }
        }
    }
}
