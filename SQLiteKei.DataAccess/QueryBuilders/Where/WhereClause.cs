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
            resultString.Append(" > ");
            resultString.Append(value);

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsGreaterThanOrEqual(object value)
        {
            resultString.Append(" >= ");
            resultString.Append(value);

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsLessThan(object value)
        {
            resultString.Append(" < ");
            resultString.Append(value);

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsLessThanOrEqual(object value)
        {
            resultString.Append(" <= ");
            resultString.Append(value);

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder IsLike(string pattern)
        {
            resultString.Append(" LIKE ");
            resultString.Append("'" + pattern + "'");

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder Contains(object value)
        {
            resultString.Append(" LIKE ");
            resultString.Append("'%" + value + "%'");

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder BeginsWith(object value)
        {
            resultString.Append(" LIKE ");
            resultString.Append("'" + value + "%'");

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }

        public ConditionalQueryBuilder EndsWith(object value)
        {
            resultString.Append(" LIKE ");
            resultString.Append("'%" + value + "'");

            queryBuilder.AddWhereClause(resultString.ToString());
            return queryBuilder;
        }
    }
}
