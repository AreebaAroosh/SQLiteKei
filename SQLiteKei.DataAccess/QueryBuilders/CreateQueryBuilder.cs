using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders.Data;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

using System.Collections.Generic;
using System.Linq;
using System;

namespace SQLiteKei.DataAccess.QueryBuilders
{
    public class CreateQueryBuilder
    {
        private string table; 

        private List<ColumnData> Columns { get; set; }

        private List<ForeignKeyData> ForeignKeys { get; set; }

        private bool primaryKeyAdded;

        public CreateQueryBuilder(string table)
        {
            this.table = table;
            Columns = new List<ColumnData>();
            ForeignKeys = new List<ForeignKeyData>();
        }

        public CreateQueryBuilder AddColumn(string columnName, DataType dataType, bool isPrimary = false, bool isNotNull = true, object defaultValue = null)
        {
            if(string.IsNullOrWhiteSpace(columnName))
            {
                throw new ColumnDefinitionException("Invalid column name.");
            }

            CheckIfColumnAlreadyAdded(columnName);

            if (isPrimary)
            {
                if (primaryKeyAdded)
                    throw new ColumnDefinitionException("Multiple primary keys defined.");

                primaryKeyAdded = true;
            }

            Columns.Add(new ColumnData
            {
                ColumnName = columnName,
                DataType = dataType,
                IsPrimary = isPrimary,
                IsNotNull = isNotNull,
                DefaultValue = defaultValue
            });

            return this;
        }

        private void CheckIfColumnAlreadyAdded(string columnName)
        {
            var result = Columns.Find(c => c.ColumnName.Equals(columnName));

            if(result != null)
            {
                var exceptionMessage =
                    string.Format("The column with the name '{0}' was defined more than once.", columnName);
                throw new ColumnDefinitionException(exceptionMessage);
            }
        }

        public CreateQueryBuilder AddForeignKey(string localColumn, string referencedTable, string referencedColumn)
        {
            CheckIfForeignKeyAlreadyAdded(localColumn);

            ForeignKeys.Add(new ForeignKeyData
            {
                LocalColumn = localColumn,
                ReferencedTable = referencedTable,
                ReferencedColumn = referencedColumn
            });

            return this;
        }

        private void CheckIfForeignKeyAlreadyAdded(string columnName)
        {
            var result = ForeignKeys.Find(c => c.LocalColumn.Equals(columnName));

            if (result != null)
            {
                var exceptionMessage =
                    string.Format("The foreign key on '{0}' was defined more than once.", columnName);
                throw new CreateQueryBuilderException(exceptionMessage);
            }
        }

        public string Build()
        {
            if(string.IsNullOrWhiteSpace(table))
            {
                throw new CreateQueryBuilderException("No valid table name provided.");
            }

            if(!Columns.Any())
            {
                throw new ColumnDefinitionException("No columns defined.");
            }

            string columns = string.Join(",\n", Columns);
            
            if(ForeignKeys.Any())
            {
                string foreignKeys = string.Join(",\n", ForeignKeys);
                return string.Format("CREATE TABLE {0}\n(\n{1},\n{2}\n);", table, columns, foreignKeys);
            }

            return string.Format("CREATE TABLE {0}\n(\n{1}\n);", table, columns);
        }
    }
}
