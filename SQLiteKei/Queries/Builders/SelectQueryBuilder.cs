#region usings

using System;

#endregion

namespace SQLiteKei.Queries.Builders
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
