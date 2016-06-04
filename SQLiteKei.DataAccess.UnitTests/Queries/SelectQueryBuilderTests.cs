#region usings

using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

#endregion

namespace SQLiteKei.UnitTests.Queries
{
    [TestFixture]
    public class SelectQueryBuilderTests
    {
        [Test]
        public void Test()
        {
            var result = QueryBuilder.Select().AddSelect("select 2").From("Table").Build();
        }
    }
}
