using System;
using System.Data.Common;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// The base class for DbHandlers which implements IDisposable and provides a DbConnection for the specified sqlite database.
    /// </summary>
    public abstract class DisposableDbHandler : IDisposable
    {
        protected DbConnection connection;

        protected DisposableDbHandler(string databasePath)
        {
            InitializeConnection(databasePath);
        }

        private void InitializeConnection(string databasePath)
        {
            var factory = DbProviderFactories.GetFactory("System.Data.SQLite");
            connection = factory.CreateConnection();

            connection.ConnectionString = string.Format("Data Source={0}", databasePath);
            connection.Open();
        }

        public void Dispose()
        {
            connection.Close();
            connection.Dispose();
        }
    }
}
