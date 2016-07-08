using System;

namespace SQLiteKei.Extensions
{
    public static class StringExtension
    {
        public static bool Contains(this string original, string value, StringComparison comparisonType)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return original.IndexOf(value, comparisonType) >= 0;
        }
    }
}
