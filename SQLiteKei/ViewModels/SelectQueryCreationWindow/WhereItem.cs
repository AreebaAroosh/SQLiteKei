using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.SelectQueryCreationWindow
{
    public class WhereItem : NotifyingItem
    {
        public string ColumnName { get; set; }

        private bool isOrClause;
        public bool IsOrClause
        {
            get { return isOrClause; }
            set { isOrClause = value; NotifyPropertyChanged("IsOrClause"); }
        }

        private bool isSelected;
        public bool IsSelected
        {
            get { return isSelected; }
            set { isSelected = value; NotifyPropertyChanged("IsSelected"); }
        }
    }
}
