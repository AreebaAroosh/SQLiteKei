#region usings

using NUnit.Framework;

using SQLiteKei.Exceptions.Queries;
using SQLiteKei.Queries.Builders;
using SQLiteKei.Queries.Enums;

#endregion

namespace SQLiteKei.UnitTests.DataHandling
{
    [TestFixture]
    public class CreateQueryBuilderTests
    {
        [Test]
        public void Build_WithValidData_ReturnsValidCommand()
        {
            const string EXPECTED_COMMAND = "CREATE TABLE Table\n(\nColumn1 Integer PRIMARY KEY NOT NULL,\nColumn2 Text NOT NULL\n);";

            var result = QueryBuilder
                .Create("Table")
                .AddColumn("Column1", DataType.Integer, true)
                .AddColumn("Column2", DataType.Text)
                .Build();

            Assert.AreEqual(EXPECTED_COMMAND, result);
        }

        [Test]
        public void Build_WithNulledTableName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create(null)
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithEmptyTableName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create(string.Empty)
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithWhitespacedTableName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("  ")
                .AddColumn("Column", DataType.Bool)
                .Build());
        }

        [Test]
        public void Build_WithoutColumn_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("Table")
                .Build());
        }

        [Test]
        public void Build_WithColumnsWithTheSameName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn("Column", DataType.Bool)
                .AddColumn("Column", DataType.Blob)
                .Build());
        }

        [Test]
        public void Build_WithMultiplePrimaryKeys_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn("Column", DataType.Bool)
                .AddColumn("Column", DataType.Blob)
                .Build());
        }

        [Test]
        public void AddColumn_WithEmptyColumnName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn(null, DataType.Bool));
        }

        [Test]
        public void AddColumn_WithNulledColumnName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn(string.Empty, DataType.Bool));
        }

        [Test]
        public void AddColumn_WithWhitespacedColumnName_ThrowsException()
        {
            Assert.Throws(typeof(QueryBuilderException),
                () => QueryBuilder.Create("Table")
                .AddColumn("  ", DataType.Bool));
        }
    }
}
