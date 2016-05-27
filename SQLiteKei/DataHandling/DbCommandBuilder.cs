namespace SQLiteKei.DataHandling
{
    public abstract class DbCommandBuilder
    {
        protected string table;

        protected string where;

        public static SelectCommandBuilder Select(string select)
        {
            return new SelectCommandBuilder(select);
        }

        public static CreateCommandBuilder Create(string tableName)
        {
            return new CreateCommandBuilder(tableName);
        }

        public abstract string Build();
    }
}
