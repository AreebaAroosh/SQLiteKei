using log4net;

using SQLiteKei.Commands;
using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.Helpers;
using SQLiteKei.ViewModels.Base;
using SQLiteKei.ViewModels.Common;

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SQLiteKei.ViewModels.TableCreatorWindow
{
    public class TableCreatorViewModel : NotifyingModel
    {
        private readonly ILog logger = LogHelper.GetLogger();

        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set
            {
                selectedDatabase = value;

                foreach (var foreignKey in ForeignKeyDefinitions)
                {
                    foreignKey.SelectedDatabasePath = selectedDatabase.DatabasePath;
                }
            }
        }

        public List<DatabaseSelectItem> Databases { get; set; }

        public ObservableCollection<ColumnDefinitionItem> ColumnDefinitions { get; set; }

        public ObservableCollection<ForeignKeyDefinitionItem> ForeignKeyDefinitions { get; set; }

        private string tableName;
        public string TableName
        {
            get { return tableName; }
            set 
            { 
                tableName = value; 
                NotifyPropertyChanged("TableName");
                UpdateSqlStatement();
            }
        }

        private string sqlStatement;
        public string SqlStatement
        {
            get { return sqlStatement; }
            set { sqlStatement = value; NotifyPropertyChanged("SqlStatement"); }
        }

        private bool isValidTableDefinition;
        public bool IsValidTableDefinition
        {
            get { return isValidTableDefinition; }
            set { isValidTableDefinition = value; NotifyPropertyChanged("IsValidTableDefinition"); }
        }

        private string statusInfo;
        public string StatusInfo
        {
            get { return statusInfo; }
            set { statusInfo = value; NotifyPropertyChanged("StatusInfo"); }
        }

        public TableCreatorViewModel()
        {
            Databases = new List<DatabaseSelectItem>();
            ColumnDefinitions = new ObservableCollection<ColumnDefinitionItem>();
            ForeignKeyDefinitions = new ObservableCollection<ForeignKeyDefinitionItem>();

            ColumnDefinitions.CollectionChanged += CollectionContentChanged;
            ForeignKeyDefinitions.CollectionChanged += CollectionContentChanged;

            addColumnCommand = new DelegateCommand(AddColumnDefinition);
            addForeignKeyCommand = new DelegateCommand(AddForeignKeyDefinition);
            createCommand = new DelegateCommand(Create);
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NotifyingModel item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NotifyingModel item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }

            var sendingModel = sender as ObservableCollection<ColumnDefinitionItem>;
            if(sendingModel != null)
            {
                UpdateAvailableColumnsForForeignKeys();
            }
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSqlStatement();

            if (e.PropertyName == "ColumnName")
                UpdateAvailableColumnsForForeignKeys();
        }

        private void UpdateSqlStatement()
        {
            StatusInfo = string.Empty;

            try
            {
                var builder = QueryBuilder.Create(tableName);

                foreach (var definition in ColumnDefinitions)
                {
                    builder.AddColumn(definition.ColumnName,
                        definition.DataType,
                        definition.IsPrimary,
                        definition.IsNotNull,
                        definition.DefaultValue);
                }

                foreach(var foreignKey in ForeignKeyDefinitions)
                {
                    if(!string.IsNullOrWhiteSpace(foreignKey.SelectedColumn)
                       && !string.IsNullOrWhiteSpace(foreignKey.SelectedTable)
                       && !string.IsNullOrWhiteSpace(foreignKey.SelectedReferencedColumn))
                    {
                        builder.AddForeignKey(foreignKey.SelectedColumn, foreignKey.SelectedTable, foreignKey.SelectedReferencedColumn);
                    }
                }

                SqlStatement = builder.Build();
                IsValidTableDefinition = true;
            }
            catch (Exception ex)
            {
                StatusInfo = ex.Message;
                IsValidTableDefinition = false;
            }
        }

        private void UpdateAvailableColumnsForForeignKeys()
        {
            foreach(var foreignKey in ForeignKeyDefinitions)
            {
                foreignKey.AvailableColumns.Clear();
                foreach (var column in ColumnDefinitions)
                {
                    foreignKey.AvailableColumns.Add(column.ColumnName);
                }
            }
        }

        private void AddColumnDefinition()
        {
            ColumnDefinitions.Add(new ColumnDefinitionItem());
        }

        private readonly DelegateCommand addColumnCommand;

        public DelegateCommand AddColumnCommand { get { return addColumnCommand; } }

        public void AddForeignKeyDefinition()
        {
            ForeignKeyDefinitionItem foreignKeyDefinition;

            if (selectedDatabase == null)
                foreignKeyDefinition = new ForeignKeyDefinitionItem(string.Empty);
            else
                foreignKeyDefinition = new ForeignKeyDefinitionItem(selectedDatabase.DatabasePath);

            foreach(var column in ColumnDefinitions)
            {
                foreignKeyDefinition.AvailableColumns.Add(column.ColumnName);
            }

            ForeignKeyDefinitions.Add(foreignKeyDefinition);
        }

        private readonly DelegateCommand addForeignKeyCommand;

        public DelegateCommand AddForeignKeyCommand { get { return addForeignKeyCommand; } }

        private void Create()
        {
            StatusInfo = string.Empty;

            if (SelectedDatabase == null)
            {
                StatusInfo = LocalisationHelper.GetString("TableCreator_NoDatabaseSelected");
                return;
            }

            if (!string.IsNullOrEmpty(SqlStatement))
            {
                var database = SelectedDatabase as DatabaseSelectItem;
                var dbHandler = new DatabaseHandler(database.DatabasePath);

                try
                {
                    if (SqlStatement.StartsWith("CREATE TABLE", StringComparison.CurrentCultureIgnoreCase))
                    {
                        dbHandler.ExecuteNonQuery(SqlStatement);
                        StatusInfo = LocalisationHelper.GetString("TableCreator_TableCreateSuccess");
                    }
                }
                catch (Exception ex)
                {
                    logger.Error("An error occured when the user tried to create a table from the TableCreator.", ex);
                    StatusInfo = ex.Message.Replace("SQL logic error or missing database\r\n", "SQL-Error - ");
                }
            }
        }

        private readonly DelegateCommand createCommand;

        public DelegateCommand CreateCommand { get { return createCommand; } }
    }
}
