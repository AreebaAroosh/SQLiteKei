using System;
using System.Collections.Generic;
using System.Text;

namespace SQLiteKei.Queries.Builders
{
    public class SelectQueryBuilder : QueryBuilder
    {
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


        public override string Build()
        {
            List<string> selectsAndAliases = new List<string>();

            foreach(var select in selects)
            {
                var stringBuilder = new StringBuilder(select.Key);

                if(!string.IsNullOrWhiteSpace(select.Value))
                {
                    stringBuilder.Append(" AS ");
                    stringBuilder.Append(select.Value);
                }
                
                selectsAndAliases.Add(stringBuilder.ToString());
            }
                
            var combinedSelect = string.Join(", ", selectsAndAliases);
            return string.Format("SELECT {0}\nFROM {1}", combinedSelect, table);
        }
    }
}
