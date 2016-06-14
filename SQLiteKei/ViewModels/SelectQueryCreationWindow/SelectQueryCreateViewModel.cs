using SQLiteKei.DataAccess.Database;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Where;
using SQLiteKei.DataAccess.QueryBuilders.Base;
using SQLiteKei.ViewModels.Base;

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System;

namespace SQLiteKei.ViewModels.SelectQueryCreationWindow
{
    /// <summary>
    /// The main ViewModel for the GenerateSelectQuery window
    /// </summary>
    public class SelectQueryCreateViewModel : NotifyingItem
    {
        private readonly string tableName;

        private SelectQueryBuilder selectQueryBuilder;

        public ObservableCollection<SelectItem> Selects { get; set; }

        private string selectQuery;
        public string SelectQuery
        {
            get { return selectQuery; }
            set { selectQuery = value; NotifyPropertyChanged("SelectQuery"); }
        }

        public SelectQueryCreateViewModel(string tableName)
        {
            this.tableName = tableName;
            Initialize();
        }

        private void Initialize()
        {
            Selects = new ObservableCollection<SelectItem>();
            Selects.CollectionChanged += CollectionContentChanged;

            InitializeItems();
            UpdateSelectQuery();
        }

        private void InitializeItems()
        {
            var databasePath = ((App)Application.Current).CurrentDatabase;
            using (var databaseHandler = new TableHandler(databasePath))
            {
                var columns = databaseHandler.GetColumns(tableName);

                foreach (var column in columns)
                {
                    Selects.Add(new SelectItem
                    {
                        ColumnName = column.Name,
                        IsSelected = true
                    });
                }
            }
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

            UpdateSelectQuery();
        }

        private void CollectionItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateSelectQuery();
        }

        private void UpdateSelectQuery()
        {
            selectQueryBuilder = new SelectQueryBuilder();

            var canBeWildcard = DetermineIfSelectCanBeWildcard();

            if (canBeWildcard)
            {
                selectQueryBuilder.AddSelect("*");
            }
            else
            {
                foreach (var select in Selects)
                {
                    if (select.IsSelected)
                    {
                        if (string.IsNullOrWhiteSpace(select.Alias))
                            selectQueryBuilder.AddSelect(select.ColumnName);
                        else
                            selectQueryBuilder.AddSelect(select.ColumnName, select.Alias);
                    }
                }
            }

            AddWhereClauses();
            SelectQuery = selectQueryBuilder.From(tableName).Build();
        }

        /// <summary>
        /// Determines if select statement can be wildcard. This is the case when all columns are selected and no aliases are defined.
        /// </summary>
        /// <returns></returns>
        private bool DetermineIfSelectCanBeWildcard()
        {
            var hasUnselectedColumns = Selects.Any(c => !c.IsSelected);
            var hasAliases = false;

            if (!hasUnselectedColumns)
                hasAliases = Selects.Any(c => !string.IsNullOrWhiteSpace(c.Alias));

            return !hasUnselectedColumns && !hasAliases;
        }

        private void AddWhereClauses()
        {
            foreach (var select in Selects)
            {
                if (!string.IsNullOrWhiteSpace(select.CompareValue))
                {
                    WhereClause where;

                    if (selectQueryBuilder.WhereClauses.Any())
                        where = selectQueryBuilder.And(select.ColumnName);
                    else
                        where = selectQueryBuilder.Where(select.ColumnName);

                    selectQueryBuilder = AddClause(where, select.CompareType, select.CompareValue) as SelectQueryBuilder;
                }
            }
        }

        private ConditionalQueryBuilder AddClause(WhereClause clause, string compareType, string compareValue)
        {
            switch(compareType)
            {
                case "=":
                    return clause.Is(compareValue);
                case ">":
                    return clause.IsGreaterThan(compareValue);
                case ">=":
                    return clause.IsGreaterThanOrEqual(compareValue);
                case "&lt;":
                    return clause.IsLessThan(compareValue);
                case "&lt;=":
                    return clause.IsLessThanOrEqual(compareValue);
                case "Like":
                    return clause.IsLike(compareValue);
                case "Contains":
                    return clause.Contains(compareValue);
                case "Begins with":
                    return clause.BeginsWith(compareValue);
                case "Ends with":
                    return clause.EndsWith(compareValue);
                default:
                    throw new NotImplementedException();
            }
        }
    }
}
