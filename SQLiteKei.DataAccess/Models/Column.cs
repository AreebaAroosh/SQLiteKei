namespace SQLiteKei.DataAccess.Models
{
    public class Column
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string DataType { get; set; }

        public bool IsNotNullable { get; set; }

        public object DefaultValue { get; set; }

        public bool IsPrimary { get; set; }
    }
}
