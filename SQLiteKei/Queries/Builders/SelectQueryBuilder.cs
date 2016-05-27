#region usings

using System;

using System.Data.Common;

#endregion

namespace SQLiteKei.DataHandling.Queries.Builders
{
    public class SelectQueryBuilder : QueryBuilder
    {
        private string select;

        public SelectQueryBuilder(string select)
        {
            this.select = select;
        }

        public SelectQueryBuilder From(string tableName)
        {
            table = tableName;
            return this;
        }

        public override string Build()
        {
            throw new NotImplementedException();
        }
    }
}
