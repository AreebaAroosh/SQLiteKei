using System;

namespace SQLiteKei.Exceptions.Queries
{
    public class CreateQueryBuilderException : Exception
    {
        public CreateQueryBuilderException(string message)
            : base("Create query could not be built. " + message)
        {
        }
    }
}
