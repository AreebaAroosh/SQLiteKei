using System.Data;

namespace SQLiteKei.DataAccess.Database
{
    public partial class DatabaseHandler
    {
        /// <summary>
        /// Executes the specified sql and returns a DataTable with the results.
        /// </summary>
        /// <param name="query">The query.</param>
        /// <returns></returns>
        public DataTable ExecuteReader(string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;

                var resultTable = new DataTable();
                resultTable.Load(command.ExecuteReader());

                return resultTable;
            } 
        }

        /// <summary>
        /// Executes the given SQL as a non query and returns the number of rows affected.
        /// </summary>
        /// <param name="sql">The SQL.</param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = sql;

                var result = command.ExecuteNonQuery();

                return result;
            }
        }
    }
}
