using SQLiteKei.DataAccess.QueryBuilders;
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
        private DatabaseSelectItem selectedDatabase;
        public DatabaseSelectItem SelectedDatabase
        {
            get { return selectedDatabase; }
            set { selectedDatabase = value; UpdateSelectedDatabaseOnForeignKeys(); }
        }

        private void UpdateSelectedDatabaseOnForeignKeys()
        {
            foreach(var foreignKey in ForeignKeyDefinitions)
            {
                foreignKey.SelectedDatabasePath = selectedDatabase.DatabasePath;
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
            sqlStatement = string.Empty;

            ColumnDefinitions.CollectionChanged += CollectionContentChanged;
            ForeignKeyDefinitions.CollectionChanged += CollectionContentChanged;
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NotifyingModel item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
                UpdateAvailableColumnsForForeignKeys();
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NotifyingModel item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
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

        public void AddColumnDefinition()
        {
            ColumnDefinitions.Add(new ColumnDefinitionItem());
        }

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
    }
}
