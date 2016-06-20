using SQLiteKei.DataAccess.QueryBuilders.Enums;

using System.Text;

namespace SQLiteKei.DataAccess.QueryBuilders.Data
{
    /// <summary>
    /// Data that is used by the CreateQueryBuilder to create the column creation statements.
    /// </summary>
    public class ColumnData
    {
        public string ColumnName { get; set; }

        public DataType DataType { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsNotNull { get; set; }

        public object DefaultValue { get; set; }

        public override string ToString()
        {
            var column = new StringBuilder(ColumnName + " " + DataType);

            if (IsPrimary)
                column.Append(" PRIMARY KEY");

            if (IsNotNull || IsPrimary)
                column.Append(" NOT NULL");

            if (DefaultValue != null)
                column.Append(" DEFAULT '" + DefaultValue + "'");

            return column.ToString();
        }
    }
}
