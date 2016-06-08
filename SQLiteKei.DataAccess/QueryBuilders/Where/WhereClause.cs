using System.Text;
using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders.Where
{
    public class WhereClause
    {
        protected ConditionalQueryBuilder queryBuilder;

        protected StringBuilder resultString;

        public WhereClause(ConditionalQueryBuilder queryBuilder, string columnName)
        {
            this.queryBuilder = queryBuilder;
            resultString = new StringBuilder(columnName);
        }

        protected WhereClause() {}

        public ConditionalQueryBuilder Is(object value)
        {
            resultString.Append(" = ");
            resultString.Append(value);

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsGreaterThan(object value)
        {
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsLessThan(object value)
        {
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsLike(object value)
        {
            return queryBuilder;
        }

        public ConditionalQueryBuilder Contains(string text)
        {
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsBetween(object from, object to)
        {
            return queryBuilder;
        }
    }
}
