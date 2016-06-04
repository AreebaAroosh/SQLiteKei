namespace SQLiteKei.Queries.Builders
{
    /// <summary>
    /// Allows to build SQL query strings 
    /// </summary>
    public abstract class QueryBuilder
    {
        protected string table;

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

        public abstract string Build();
    }
}
