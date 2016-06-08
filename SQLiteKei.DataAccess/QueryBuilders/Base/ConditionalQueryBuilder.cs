using SQLiteKei.DataAccess.QueryBuilders.Where;

namespace SQLiteKei.DataAccess.QueryBuilders.Base
{
    public abstract class ConditionalQueryBuilder : QueryBuilderBase
    {
        public abstract WhereClause Where(string columnName);

        public abstract WhereClause Or(string columnName);

        public abstract WhereClause And(string columnName);

        internal abstract void AddWhereClause(string where);
    }
}