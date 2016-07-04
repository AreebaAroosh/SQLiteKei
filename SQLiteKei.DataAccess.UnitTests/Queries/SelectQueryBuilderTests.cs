#region usings

using NUnit.Framework;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders;

#endregion

namespace SQLiteKei.UnitTests.Queries
{
    [TestFixture]
    public class SelectQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT Column1, Column2 AS Alias\nFROM Table\nWHERE Column1 = 2\nOR Column2 = 3\nORDER BY Column2 DESC, Column1";

            var result = QueryBuilder.Select("Column1")
                .AddSelect("Column2", "Alias")
                .From("Table")
                .Where("Column1").Is(2)
                .Or("Column2").Is(3)
                .OrderBy("Column2", true)
                .OrderBy("Column1")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithWhereStatement_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column1 = 1";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column1").Is(1)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithAndStatement_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column1 = 1\nAND Column2 = 2";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column1").Is(1)
                .And("Column2").Is(2)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithOrStatement_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column1 = 1\nOR Column2 = 2";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column1").Is(1)
                .Or("Column2").Is(2)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column = 1";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").Is(1)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsGreaterThanCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column > 1";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").IsGreaterThan(1)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsGreaterThanOrEqualCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column >= 1";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").IsGreaterThanOrEqual(1)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsLessThanCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column < 1";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").IsLessThan(1)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsLessThanOrEqualCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column <= 1";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").IsLessThanOrEqual(1)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsLikeCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column LIKE '%__AB'";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").IsLike("%__AB")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithIsContainsCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column LIKE '%Text%'";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").Contains("Text")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithBeginsWithCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column LIKE 'Text%'";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").BeginsWith("Text")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithEndsWithCondition_BuildsValidQuery()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table\nWHERE Column LIKE '%Text'";

            var result = QueryBuilder.Select()
                .From("Table")
                .Where("Column").EndsWith("Text")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithEmptySelects_DefaultsToWildcardSelect()
        {
            const string EXPECTED_QUERY = "SELECT *\nFROM Table";

            var result = QueryBuilder.Select()
                .From("Table")
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithMoreThanOneSimpleWhereStatement_ThrowsException()
        {
            Assert.Throws(typeof (SelectQueryBuilderException),
                () => QueryBuilder.Select("Column")
                    .Where("First").Is("Test")
                    .Where("Second").Is("Test")
                    .Build());
        }
    }
}