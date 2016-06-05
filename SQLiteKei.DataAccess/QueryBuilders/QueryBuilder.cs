namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// Allows to build SQL query strings 
    /// </summary>
    public abstract class QueryBuilder
    {
        public static SelectQueryBuilder Select()
        {
            return new SelectQueryBuilder();
        }

        public static SelectQueryBuilder Select(string select)
        {
            return new SelectQueryBuilder(select);
        }

        public static SelectQueryBuilder Select(string select, string alias)
        {
            return new SelectQueryBuilder(select, alias);
        }

        public static CreateQueryBuilder Create(string tableName)
        {
            return new CreateQueryBuilder(tableName);
        }

        public static DropQueryBuilder Drop(string tableName)
        {
            return new DropQueryBuilder(tableName);
        }

        public abstract string Build();
    }
}
