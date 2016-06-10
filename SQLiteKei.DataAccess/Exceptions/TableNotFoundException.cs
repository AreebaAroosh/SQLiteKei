using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException(string tableName)
            : base("Table could not be found: " + tableName)
        {
        }
    }
}
