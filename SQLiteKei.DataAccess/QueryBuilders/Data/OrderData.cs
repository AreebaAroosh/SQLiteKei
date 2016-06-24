namespace SQLiteKei.DataAccess.QueryBuilders.Data
{
    internal class OrderData
    {
        public string ColumnName { get; set; }

        public bool IsDescending { get; set; }

        public override string ToString()
        {
            if (!IsDescending)
                return ColumnName;

            return string.Format("{0} DESC", ColumnName);
        }
    }
}
