#region usings

using NUnit.Framework;

using SQLiteKei.DataAccess.QueryBuilders;

#endregion

namespace SQLiteKei.DataAccess.UnitTests.Queries
{
    [TestFixture]
    public class DropQueryBuilderTests
    {
        [Test]
        public void Build_SimpleDrop_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE Table";
            var result = QueryBuilder.Drop("Table").Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIfExistsCase_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE IF EXISTS Table";

            var result = QueryBuilder.Drop("Table").IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithCascadeCase_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE Table CASCADE";
            var result = QueryBuilder.Drop("Table").Cascade().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithAllCases_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE IF EXISTS Table CASCADE";
            var result = QueryBuilder.Drop("Table").IfExists().Cascade().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithMultipleIfExistsCases_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE IF EXISTS Table";

            var result = QueryBuilder.Drop("Table").IfExists().IfExists().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithMultipleCascadeCases_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "DROP TABLE Table CASCADE";

            var result = QueryBuilder.Drop("Table").Cascade().Cascade().Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }
    }
}
