using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    public class TableNotFoundException : Exception
    {
        public TableNotFoundException(string message) : base(message)
        {
        }
    }
}
