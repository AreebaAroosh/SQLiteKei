namespace SQLiteKei.Helpers
{
    /// <summary>
    /// A helper class that loads the localized string with the specified key (refer to Resources.-Files for keys).
    /// </summary>
    public static class LocalisationHelper
    {
        /// <summary>
        /// Gets the localized string for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public static string GetString(string key)
        {
            return Properties.Resources.ResourceManager.GetObject(key) as string;
        }

        /// <summary>
        /// Gets the localized for the specified key. If the key accepts parameters, the given arguments will be included in the string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="args">The arguments.</param>
        /// <returns></returns>
        public static string GetString(string key, params object[] args)
        {
            var localizedString = Properties.Resources.ResourceManager.GetObject(key) as string;
            return string.Format(localizedString, args);
        }
    }
}
