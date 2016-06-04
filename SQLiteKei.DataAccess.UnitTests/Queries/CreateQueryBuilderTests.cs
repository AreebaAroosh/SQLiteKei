#region usings

using NUnit.Framework;

using SQLiteKei.DataAccess.Exceptions;
using SQLiteKei.DataAccess.QueryBuilders;
using SQLiteKei.DataAccess.QueryBuilders.Enums;

#endregion

namespace SQLiteKei.UnitTests.Queries
{
    [TestFixture]
    public class CreateQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidCommand()
        {
            const string EXPECTED_QUERY = "CREATE TABLE Table\n(\nColumn1 Integer PRIMARY KEY NOT NULL,\nColumn2 Text NOT NULL\n);";

            var result = QueryBuilder
                .Create("Table")
                .AddColumn("Column1", DataType.Integer, true)
                .AddColumn("Column2", DataType.Text)
                .Build();

            Assert.AreEqual(EXPECTED_QUERY, result);
        }

        [Test]
        public void Build_WithNulledTableName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create(null)
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithEmptyTableName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create(string.Empty)
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithWhitespacedTableName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("  ")
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithoutColumn_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("Table")
                .Build());
        }

        [Test]
        public void Build_WithColumnsWithTheSameName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn("Column", DataType.Bool)
                .AddColumn("Column", DataType.Blob)
                .Build());
        }

        [Test]
        public void Build_WithMultiplePrimaryKeys_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn("Column", DataType.Bool, true)
                .AddColumn("Column1", DataType.Blob, true)
                .Build());
        }

        [Test]
        public void AddColumn_WithEmptyColumnName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn(null, DataType.Bool));
        }

        [Test]
        public void AddColumn_WithNulledColumnName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn(string.Empty, DataType.Bool));
        }

        [Test]
        public void AddColumn_WithWhitespacedColumnName_ThrowsException()
        {
            Assert.Throws(typeof(CreateQueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn("  ", DataType.Bool));
        }
    }
}
