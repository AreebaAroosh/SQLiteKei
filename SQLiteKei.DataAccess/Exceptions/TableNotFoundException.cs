using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    /// <summary>
    /// Exception that is thrown when a table could not be found in the database.
    /// </summary>
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException(string tableName)
            : base("Table could not be found: " + tableName)
        {
        }
    }
}
