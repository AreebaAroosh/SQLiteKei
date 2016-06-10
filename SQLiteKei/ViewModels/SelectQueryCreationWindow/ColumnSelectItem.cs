using System.ComponentModel;

namespace SQLiteKei.ViewModels.SelectQueryCreationWindow
{
    /// <summary>
    /// A view model that is used to represent a build a user's select statement.
    /// </summary>
    public class ColumnSelectItem : INotifyPropertyChanged
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

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
