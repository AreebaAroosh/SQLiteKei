namespace SQLiteKei.DataAccess.QueryBuilders.Data
{
    internal class ForeignKeyData
    {
        public string LocalColumn { get; set; }
        
        public string ReferencedTable { get; set; }

        public string ReferencedColumn { get; set; }

        public override string ToString()
        {
            return string.Format("FOREIGN KEY({0}) REFERENCES {1}({2})", LocalColumn, ReferencedTable, ReferencedColumn);
        }
    }
}
