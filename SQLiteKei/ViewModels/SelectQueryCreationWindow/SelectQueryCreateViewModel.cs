﻿using SQLiteKei.DataAccess.QueryBuilders;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SQLiteKei.ViewModels.SelectQueryCreationWindow
{
    /// <summary>
    /// The main ViewModel for the GenerateSelectQuery window
    /// </summary>
    public class SelectQueryCreateViewModel : INotifyPropertyChanged
    {
        private readonly string tableName;

        public ObservableCollection<ColumnSelectItem> Columns { get; set; }

        public string TableName { get; set; }

        private string selectQuery;
        public string SelectQuery
        {
            get { return selectQuery; }
            set { selectQuery = value; NotifyPropertyChanged("SelectQuery"); }
        }

        public SelectQueryCreateViewModel(string tableName)
        {
            this.tableName = tableName;

            Columns = new ObservableCollection<ColumnSelectItem>();
            Columns.CollectionChanged += CollectionContentChanged;
            UpdateSelectQuery();
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (ColumnSelectItem item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (ColumnSelectItem item in e.NewItems)
                {
                    item.PropertyChanged += CollectionItemPropertyChanged;
                }
            }

            UpdateSelectQuery();
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSelectQuery();
        }

        private void UpdateSelectQuery()
        {
            SelectQueryBuilder selectQueryBuilder = QueryBuilder.Select();

            var canBeWildcard = DetermineIfSelectCanBeWildcard();

            if (canBeWildcard)
            {
                selectQueryBuilder.AddSelect("*");
            }
            else
            {
                foreach (var column in Columns)
                {
                    if (column.IsSelected)
                    {
                        if (string.IsNullOrWhiteSpace(column.Alias))
                            selectQueryBuilder.AddSelect(column.ColumnName);
                        else
                            selectQueryBuilder.AddSelect(column.ColumnName, column.Alias);
                    }
                }
            }                
                
            SelectQuery = selectQueryBuilder.From(tableName).Build();
        }

        /// <summary>
        /// Determines if select statement can be wildcard. This is the case when all columns are selected and no aliases are defined.
        /// </summary>
        /// <returns></returns>
        private bool DetermineIfSelectCanBeWildcard()
        {
            var hasUnselectedColumns = Columns.Any(c => !c.IsSelected);
            var hasAliases = false;

            if (!hasUnselectedColumns)
                hasAliases = Columns.Any(c => !string.IsNullOrWhiteSpace(c.Alias));

            return !hasUnselectedColumns && !hasAliases;
        }

        #region INotifyPropertyChanged Members
        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }
        #endregion
    }
}
