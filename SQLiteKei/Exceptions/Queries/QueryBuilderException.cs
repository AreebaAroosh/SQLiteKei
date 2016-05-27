using System;

namespace SQLiteKei.Exceptions.DataHandling
{
    public class QueryBuilderException : Exception
    {
        public QueryBuilderException(string message)
            : base(message)
        {
        }
    }
}
