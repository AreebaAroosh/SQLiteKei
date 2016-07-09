namespace SQLiteKei.ViewModels.Common
{
    /// <summary>
    /// A ViewModel that resembles a selectable database which is used in ComboBoxes.
    /// </summary>
    public class DatabaseSelectItem
    {
        public string DatabaseName { get; set; }

        public string DatabasePath { get; set; }
    }
}
