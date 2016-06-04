using System.Collections.Generic;

namespace SQLiteKei.DataAccess.Database
{
    /// <summary>
    /// A class used for calls to the currently selected database.
    /// </summary>
    public static class Database
    {
        public static string DatabasePath { get; set; }

        public static List<string> GetColumnNames { get; set; }
    }
}

