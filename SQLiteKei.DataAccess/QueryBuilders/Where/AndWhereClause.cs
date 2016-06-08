using System.Text;
using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders.Where
{
    public class AndWhereClause : WhereClause
    {
        public AndWhereClause(ConditionalQueryBuilder queryBuilder, string columnName)
        {
            this.queryBuilder = queryBuilder;
            resultString = new StringBuilder("AND " + columnName);
        }
    }
}