using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.ViewModels.Base;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace SQLiteKei.ViewModels.SelectQueryCreationWindow
{
    /// <summary>
    /// The main ViewModel for the GenerateSelectQuery window
    /// </summary>
    public class SelectQueryCreateViewModel : NotifyingItem
    {
        private readonly string tableName;

        public ObservableCollection<SelectItem> Selects { get; set; }

        public ObservableCollection<WhereItem> WhereClauses { get; set; }

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

            Selects = new ObservableCollection<SelectItem>();
            Selects.CollectionChanged += CollectionContentChanged;
            UpdateSelectQuery();
        }

        private void CollectionContentChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (SelectItem item in e.OldItems)
                {
                    item.PropertyChanged -= CollectionItemPropertyChanged;
                }
            }
            else if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (SelectItem item in e.NewItems)
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
    }
}
