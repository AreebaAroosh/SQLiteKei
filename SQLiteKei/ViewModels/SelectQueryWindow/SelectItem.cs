using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.SelectQueryWindow
{
    /// <summary>
    /// A view model that is used to represent a build a user's select statement.
    /// </summary>
    public class SelectItem : NotifyingModel
    {
        public string ColumnName { get; set; }

        private string alias;
        public string Alias
        {
            get { return alias; }
            set { alias = value; NotifyPropertyChanged("Alias"); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; NotifyPropertyChanged("IsSelected"); }
        }

        private string compareType;
        public string CompareType
        {
            get { return compareType; }
            set { compareType = value; NotifyPropertyChanged("CompareType"); }
        }

        private string compareValue;
        public string CompareValue
        {
            get { return compareValue; }
            set { compareValue = value; NotifyPropertyChanged("CompareValue"); }
        }
    }
}
