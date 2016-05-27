#region usings

using System;

using System.Data.Common;

#endregion

namespace SQLiteKei.DataHandling
{
    public class SelectCommandBuilder : DbCommandBuilder
    {
        private string select;

        public SelectCommandBuilder(string select)
        {
            this.select = select;
        }

        public SelectCommandBuilder From(string tableName)
        {
            table = tableName;
            return this;
        }

        public override string Build()
        {
            throw new NotImplementedException();
        }
    }
}
