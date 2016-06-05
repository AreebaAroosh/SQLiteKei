#region usings

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders.Data;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

using System.Collections.Generic;
using System.Linq;

#endregion

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class CreateQueryBuilder
    {
        private string table; 

        private List<ColumnData> Columns { get; set; }

        private bool primaryKeyAdded;

        public CreateQueryBuilder(string table)
        {
            this.table = table;
            Columns = new List<ColumnData>();
        }

        public CreateQueryBuilder AddColumn(string columnName, DataType dataType, bool isPrimary = false, bool isNullable = false)
        {
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new CreateQueryBuilderException("A provided column name was null or empty.");
            }

            CheckIfColumnAlreadyAdded(columnName);

            if(isPrimary)
            {
                if (primaryKeyAdded)
                    throw new CreateQueryBuilderException("Multiple primary keys were defined.");

                primaryKeyAdded = true;
            }

            var columnData = new ColumnData
            {
                ColumnName = columnName,
                DataType = dataType,
                IsPrimary = isPrimary,
                IsNullable = isNullable
            };

            Columns.Add(columnData);
            return this;
        }

        private void CheckIfColumnAlreadyAdded(string columnName)
        {
            var result = Columns.Find(c => c.ColumnName.Equals(columnName));

            if(result != null)
            {
                throw new CreateQueryBuilderException("A column has was provided multiple times.");
            }
        }

        public string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
            {
                throw new CreateQueryBuilderException("An empty or invalid table name has been provided.");
            }

            if(!Columns.Any())
            {
                throw new CreateQueryBuilderException("No columns were provided.");
            }

            string columns = string.Join(",\n", Columns);

            return string.Format("CREATE TABLE {0}\n(\n{1}\n);", table, columns);
        }
    }
}
