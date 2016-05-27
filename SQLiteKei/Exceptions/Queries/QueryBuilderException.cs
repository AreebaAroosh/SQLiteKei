using System;

namespace SQLiteKei.Exceptions.Queries
{
    public class QueryBuilderException : Exception
    {
        public QueryBuilderException(string message)
            : base(message)
        {
        }
    }
}
