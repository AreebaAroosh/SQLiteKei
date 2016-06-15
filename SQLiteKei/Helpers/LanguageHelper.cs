namespace SQLiteKei.Helpers
{
    public static class LanguageHelper
    {
        public static string GetLocalizedString(string key)
        {
            return Properties.Resources.ResourceManager.GetObject(key) as string;
        }

        public static string GetLocalizedString(string key, params object[] args)
        {
            var localizedString = Properties.Resources.ResourceManager.GetObject(key) as string;
            return string.Format(localizedString, args);
        }
    }
}
