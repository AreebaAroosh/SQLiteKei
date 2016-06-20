using log4net;

using SQLiteKei.DataAccess.Exceptions;
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
    public class TableCreatorViewModel : NotifyingItem
    {
        public List<DatabaseSelectItem> Databases { get; set; }

        public ObservableCollection<ColumnDefinitionItem> ColumnDefinitions { get; set; }

        private ILog logger = LogHelper.GetLogger();

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
            sqlStatement = string.Empty;

            ColumnDefinitions.CollectionChanged += CollectionContentChanged;
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (NotifyingItem item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (NotifyingItem item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSqlStatement();
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
            catch (ColumnDefinitionException ex)
            {
                IsValidTableDefinition = false;
                StatusInfo = ex.Message;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                StatusInfo = ex.Message;
                IsValidTableDefinition = false;
            }
        }

        public void AddColumnDefinition()
        {
            ColumnDefinitions.Add(new ColumnDefinitionItem());
        }
    }
}
