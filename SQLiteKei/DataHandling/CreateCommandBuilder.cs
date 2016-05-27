#region usings

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using System.Windows;

#endregion

namespace SQLiteKei.DataHandling
{
    public class CreateCommandBuilder : DbCommandBuilder
    {
        private List<string> Columns { get; set; } 

        public CreateCommandBuilder(string table)
        {
            this.table = table;
            Columns = new List<string>();
        }

        public CreateCommandBuilder AddColumn(string columnName, DataType dataType, bool isPrimary = false, bool isNullable = false)
        {
            var column = new StringBuilder(columnName + " " + dataType);

            if (isPrimary)
                column.Append(" PRIMARY KEY");

            if (!isNullable || isPrimary)
                column.Append(" NOT NULL");

            Columns.Add(column.ToString());
            return this;
        }

        public override string Build()
        {
            return string.Join(",\n", Columns);
        }
    }
}
