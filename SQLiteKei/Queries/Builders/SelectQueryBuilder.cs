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
            return string.Format("SELECT {0} FROM {1}", select, table);
        }
    }
}
