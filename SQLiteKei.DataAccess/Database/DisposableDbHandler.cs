using System;
using System.Data.SQLite;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// The base class for DbHandlers which implements IDisposable and provides a DbConnection for the specified sqlite database.
    /// </summary>
    public abstract class DisposableDbHandler : IDisposable
    {
        protected SQLiteConnection connection;

        protected DisposableDbHandler(string databasePath)
        {
            InitializeConnection(databasePath);
        }

        private void InitializeConnection(string databasePath)
        {
            connection = new SQLiteConnection(databasePath)
            {
                ConnectionString = string.Format("Data Source={0}", databasePath)
            };

            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
