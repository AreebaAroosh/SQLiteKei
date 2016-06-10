namespace SQLiteKei.ViewModels.DBTreeView.Base
{
    /// <summary>
    /// The base class for tree view items.
    /// </summary>
    public abstract class TreeItem
    {
        /// <summary>
        /// Gets or sets the name that is displayed in the tree view.
        /// </summary>
        /// <value>
        /// The display name.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Gets or sets the database path. This is used to determine the current database context when a tree item is clicked.
        /// </summary>
        /// <value>
        /// The database path.
        /// </value>
        public string DatabasePath { get; set; }
    }
}