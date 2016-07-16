using log4net;
using System.Reflection;
using System.Windows;
using System.Windows.Markup;

namespace SQLiteKei.Helpers
{
    public class ThemeHelper
    {
        private static readonly ILog log = LogHelper.GetLogger();

        /// <summary>
        /// Loads the user theme defined in the user settings.
        /// </summary>
        public static void LoadCurrentUserTheme()
        {
            LoadTheme(Properties.Settings.Default.UITheme);
        }

        /// <summary>
        /// Loads the theme.
        /// </summary>
        /// <param name="themeName">Name of the theme.</param>
        public static void LoadTheme(string themeName)
        {
            log.Info("Loading '" + themeName + "' theme.");
            var themePath = string.Format("SQLiteKei.Resources.Themes.{0}.{0}.xaml", themeName);

            using (var s = Assembly.GetExecutingAssembly().GetManifestResourceStream(themePath))
            {
                ResourceDictionary dic = (ResourceDictionary)XamlReader.Load(s);

                Application.Current.Resources.MergedDictionaries.Clear();
                Application.Current.Resources.MergedDictionaries.Add(dic);
            }
        }
    }
}
