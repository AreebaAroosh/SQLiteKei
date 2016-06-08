using System.Text;
using SQLiteKei.DataAccess.QueryBuilders.Base;

namespace SQLiteKei.DataAccess.QueryBuilders.Where
{
    public class OrWhereClause : WhereClause
    {
        public OrWhereClause(ConditionalQueryBuilder queryBuilder, string columnName)
        {
            this.queryBuilder = queryBuilder;
            resultString = new StringBuilder("OR " + columnName);
        }
    }
}
