using SQLiteKei.Extensions;
using SQLiteKei.ViewModels.Base;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Data;

namespace SQLiteKei.ViewModels.MainTabControl.Tables
{
    public class RecordsTabViewModel : NotifyingModel
    {
        private ICollectionView dataGridCollection;
        public ICollectionView DataGridCollection
        {
            get { return dataGridCollection; }
            set
            {
                dataGridCollection = value;
                if (dataGridCollection.CanFilter)
                    dataGridCollection.Filter = Filter;
                NotifyPropertyChanged("DataGridCollection"); 
            }
        }

        private string searchString;
        public string SearchString
        {
            get { return searchString; }
            set
            {
                searchString = value;
                NotifyPropertyChanged("SearchString");
                FilterCollection();
            }
        }

        private void FilterCollection()
        {
            if (dataGridCollection != null)
            {
                dataGridCollection.Refresh();
            }
        }

        public RecordsTabViewModel()
        {
            DataGridCollection = new ListCollectionView(new List<object>());
        }

        private bool Filter(object obj)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return true;
            }

            var rowView = obj as DataRowView;
            var row = rowView.Row;

            return row.ItemArray.Select(entry => entry.ToString())
                .Any(value => value.Contains(searchString, StringComparison.OrdinalIgnoreCase));
        }
    }
}
