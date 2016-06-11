using System.ComponentModel;
using System.Windows;

namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private string currentDatabase { get; set; }
        public string CurrentDatabase
        {
            get { return currentDatabase; }
            set { currentDatabase = value; }
        }

    }
}
