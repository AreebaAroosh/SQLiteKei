#region usings

using SQLiteKei.Exceptions.DataHandling;
using SQLiteKei.Queries.Enums;

using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace SQLiteKei.Queries.Builders
{
    public class CreateQueryBuilder : QueryBuilder
    {
        private List<string> Columns { get; set; } 

        public CreateQueryBuilder(string table)
        {
            this.table = table;
            Columns = new List<string>();
        }

        public CreateQueryBuilder AddColumn(string columnName, DataType dataType, bool isPrimary = false, bool isNullable = false)
        {
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new QueryBuilderException("Table could not be created. One or more column names were null or empty.");
            }

            var cleanColumnName = Regex.Replace(columnName, @"\s+", "");
            var column = new StringBuilder(cleanColumnName + " " + dataType);

            if (isPrimary)
                column.Append(" PRIMARY KEY");

            if (!isNullable || isPrimary)
                column.Append(" NOT NULL");

            Columns.Add(column.ToString());
            return this;
        }

        public override string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
            {
                throw new QueryBuilderException("Table could not be created. An invalid table name has been provided.");
            }

            string columns = string.Join(",\n", Columns);

            return string.Format("CREATE TABLE {0}\n(\n{1}\n);", table, columns);
        }
    }
}
