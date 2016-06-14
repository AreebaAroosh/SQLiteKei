namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private string currentDatabase { get; set; }
        public string CurrentDatabase
        {
            get { return currentDatabase; }
            set { currentDatabase = value; }
        }
    }
}
