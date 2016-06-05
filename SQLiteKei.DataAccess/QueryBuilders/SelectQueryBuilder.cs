﻿#region usings

using System;
using System.Collections.Generic;
using System.Text;

#endregion

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class SelectQueryBuilder
    {
        private string table;

        private Dictionary<string, string> selects;

        public SelectQueryBuilder()
        {
            selects = new Dictionary<string, string>();
        }

        public SelectQueryBuilder(string select)
        {
            selects = new Dictionary<string, string>();
            selects.Add(select, string.Empty);
        }

        public SelectQueryBuilder(string select, string alias)
        {
            selects = new Dictionary<string, string>();
            selects.Add(select, alias);
        }

        public SelectQueryBuilder AddSelect(string select)
        {
            selects.Add(select, string.Empty);

            return this;
        }

        public SelectQueryBuilder AddSelect(string select, string alias)
        {
            selects.Add(select, alias);
            return this;
        }

        public SelectQueryBuilder From(string tableName)
        {
            table = tableName;
            return this;
        }

        public SelectQueryBuilder AddWhere(string where)
        {
            throw new NotImplementedException();
        }

        public string Build()
        {
            var combinedSelect = GenerateCombinedSelect();

            return string.Format("SELECT {0}\nFROM {1}", combinedSelect, table);
        }

        private string GenerateCombinedSelect()
        {
            var selectsAndAliases = new List<string>();

            foreach (var select in selects)
            {
                var stringBuilder = new StringBuilder(select.Key);

                if (!string.IsNullOrWhiteSpace(select.Value))
                {
                    stringBuilder.Append(" AS ");
                    stringBuilder.Append(select.Value);
                }

                selectsAndAliases.Add(stringBuilder.ToString());
            }

            return string.Join(", ", selectsAndAliases);
        }
    }
}