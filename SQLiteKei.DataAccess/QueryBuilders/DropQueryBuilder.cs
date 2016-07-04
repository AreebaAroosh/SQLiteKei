using System.Text;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class DropQueryBuilder
    {
        private string table;

        private bool isCascade;

        private bool isIfExists;

        public DropQueryBuilder(string tableName)
        {
            table = tableName;
        }

        public DropQueryBuilder IfExists()
        {
            isIfExists = true;

            return this;
        }

        public DropQueryBuilder Cascade()
        {
            isCascade = true;

            return this;
        }

        public string Build()
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append("DROP TABLE");

            if (isIfExists)
                stringBuilder.Append(" IF EXISTS");

            stringBuilder.Append(" '" + table + "'");

            if (isCascade)
                stringBuilder.Append(" CASCADE");

            return stringBuilder.ToString();
        }
    }
}
