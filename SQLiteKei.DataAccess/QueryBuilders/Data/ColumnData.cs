#region usings

using SQLiteKei.DataAccess.QueryBuilders.Enums;

using System.Text;
using System.Text.RegularExpressions;

#endregion

namespace SQLiteKei.DataAccess.QueryBuilders.Data
{
    /// <summary>
    /// Data that is used by the CreateQueryBuilder to create the column creation statements.
    /// </summary>
    public class ColumnData
    {
        private string columnName; 
        public string ColumnName
        {
            get
            {
                return columnName;
            }
            set
            {
                columnName = Regex.Replace(value, @"\s+", ""); // remove whitespaces
            }
        }

        public DataType DataType { get; set; }

        public bool IsPrimary { get; set; }

        public bool IsNullable { get; set; }

        public override string ToString()
        {
            var column = new StringBuilder(ColumnName + " " + DataType);

            if (IsPrimary)
                column.Append(" PRIMARY KEY");

            if (!IsNullable || IsPrimary)
                column.Append(" NOT NULL");

            return column.ToString();
        }
    }
}
