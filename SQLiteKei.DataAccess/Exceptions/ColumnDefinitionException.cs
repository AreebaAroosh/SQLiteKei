using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    public class ColumnDefinitionException : Exception
    {
        public ColumnDefinitionException(string message)
            : base(message)
        {
        }
    }
}
