using System.ComponentModel;

namespace SQLiteKei.ViewModels.Base
{
    /// <summary>
    /// An object that implements the INotifyPropertyChanged interface
    /// </summary>
    public abstract class NotifyingItem : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
    }
}
