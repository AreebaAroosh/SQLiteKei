#region usings

using NUnit.Framework;
using SQLiteKei.DataHandling;

#endregion

namespace SQLiteKei.UnitTests.DataHandling
{
    [TestFixture]
    public class DbCommandBuilderTests
    {
        [Test]
        public void Build_WithValidStatement_BuildsValidString()
        {
            var t1 = DbCommandBuilder.Create("table")
            .AddColumn("Column 1", DataType.Integer, true)
            .AddColumn("Column 2", DataType.Text)
            .Build();
        }
    }
}
