using log4net;

using System.Runtime.CompilerServices;

namespace SQLiteKei.DataAccess.Helpers
{
    public static class LogHelper
    {
        public static ILog GetLogger([CallerFilePath]string fileName = "")
        {
            return LogManager.GetLogger(fileName);
        }
    }
}
