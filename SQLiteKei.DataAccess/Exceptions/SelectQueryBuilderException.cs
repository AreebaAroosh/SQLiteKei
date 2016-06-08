using System;

namespace SQLiteKei.DataAccess.Exceptions
{
    public class SelectQueryBuilderException : Exception
    {
        public SelectQueryBuilderException(string message)
            : base("Select query could not be built. " + message)
        {
        }
    }
}
