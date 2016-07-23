using SQLiteKei.ViewModels.Base;
using SQLiteKei.DataAccess.Database;

using System.Collections.ObjectModel;

namespace SQLiteKei.ViewModels.TableCreatorWindow
{
    /// <summary>
    /// ViewModel item that defines a foreign key.
    /// </summary>
    public class ForeignKeyDefinitionItem : NotifyingModel
    {
        private string selectedDatabasePath;
        public string SelectedDatabasePath
        {
            get { return selectedDatabasePath; }
            set { selectedDatabasePath = value; UpdateReferencableTables(); }
        }

        private string selectedColumn;
        public string SelectedColumn
        {
            get { return selectedColumn; }
            set { selectedColumn = value; NotifyPropertyChanged("SelectedColumn"); }
        }

        private ObservableCollection<string> availableColumns;
        public ObservableCollection<string> AvailableColumns
        {
            get { return availableColumns; }
            set { availableColumns = value; NotifyPropertyChanged("AvailableColumns"); }
        }

        private string selectedTable { get; set; }
        public string SelectedTable
        {
            get { return selectedTable; }
            set { selectedTable = value; UpdateReferencableColumns(); }
        }

        private void UpdateReferencableColumns()
        {
            if (string.IsNullOrEmpty(SelectedDatabasePath)) return;

            using (var tableHandler = new TableHandler(SelectedDatabasePath))
            {
                var columns = tableHandler.GetColumns(selectedTable);
                {
                    ReferencableColumns.Clear();
                    
                    foreach(var column in columns)
                    {
                        ReferencableColumns.Add(column.Name);
                    }
                }
            }
        }

        public void UpdateReferencableTables()
        {
            if (string.IsNullOrEmpty(SelectedDatabasePath)) return;

            using (var dbHandler = new DatabaseHandler(SelectedDatabasePath))
            {
                var tables = dbHandler.GetTables();
                ReferencableTables.Clear();

                foreach (var table in tables)
                {
                    ReferencableTables.Add(table.Name);
                }
            }
        }

        private ObservableCollection<string> availableTables;
        public ObservableCollection<string> ReferencableTables
        {
            get { return availableTables; }
            set { availableTables = value; NotifyPropertyChanged("ReferencableTables"); }
        }

        private string selectedReferencedColumn;
        public string SelectedReferencedColumn
        {
            get { return selectedReferencedColumn; }
            set { selectedReferencedColumn = value; NotifyPropertyChanged("SelectedReferencedColumn"); }
        }

        private ObservableCollection<string> referencableColumns;
        public ObservableCollection<string> ReferencableColumns
        {
            get { return referencableColumns; }
            set { referencableColumns = value;  NotifyPropertyChanged("ReferencableColumns"); }
        }

        public ForeignKeyDefinitionItem(string selectedDatabasePath)
        {
            AvailableColumns = new ObservableCollection<string>();
            ReferencableTables = new ObservableCollection<string>();
            ReferencableColumns = new ObservableCollection<string>();

            SelectedDatabasePath = selectedDatabasePath;

            UpdateReferencableTables();
        }
    }
}