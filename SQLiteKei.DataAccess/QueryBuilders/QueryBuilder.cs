namespace SQLiteKei.DataAccess.QueryBuilders
{
    /// <summary>
    /// Allows to build SQL query strings.
    /// </summary>
    public abstract class QueryBuilder
    {
        /// <summary>
        /// Begins a SELECT statement. If no further columns are defined, the statement will be created using a wildcard.
        /// </summary>
        public static SelectQueryBuilder Select()
        {
            return new SelectQueryBuilder();
        }

        /// <summary>
        /// Begins a SELECT statement.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <returns></returns>
        public static SelectQueryBuilder Select(string column)
        {
            return new SelectQueryBuilder(column);
        }

        /// <summary>
        /// Begins a SELECT statement. If no columns are defined, it defaults to a wildcard selection.
        /// </summary>
        /// <param name="column">The column.</param>
        /// <param name="alias">The alias.</param>
        /// <returns></returns>
        public static SelectQueryBuilder Select(string column, string alias)
        {
            return new SelectQueryBuilder(column, alias);
        }

        /// <summary>
        /// Begins a CREATE statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static CreateQueryBuilder Create(string tableName)
        {
            return new CreateQueryBuilder(tableName);
        }

        /// <summary>
        /// Begins a DROP statement for the specified table.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <returns></returns>
        public static DropQueryBuilder Drop(string tableName)
        {
            return new DropQueryBuilder(tableName);
        }
    }
}
