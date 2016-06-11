using SQLiteKei.ViewModels.Base;

namespace SQLiteKei.ViewModels.SelectQueryCreationWindow
{
    /// <summary>
    /// A view model that is used to represent a build a user's select statement.
    /// </summary>
    public class SelectItem : NotifyingItem
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
    }
}
