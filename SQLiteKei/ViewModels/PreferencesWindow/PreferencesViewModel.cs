using log4net;

using SQLiteKei.Helpers;

using System.Collections.Generic;

namespace SQLiteKei.ViewModels.PreferencesWindow
{
    public class PreferencesViewModel
    {
        private readonly ILog log = LogHelper.GetLogger();

        public List<string> AvailableLanguages { get; set; }

        private string selectedLanguage;
        public string SelectedLanguage
        {
            get
            {
                if (string.IsNullOrEmpty(selectedLanguage))
                    selectedLanguage = GetLanguageFromSettings();

                return selectedLanguage;
            }
            set { selectedLanguage = value; }
        }

        private string GetLanguageFromSettings()
        {
            var setting = Properties.Settings.Default.UILanguage;

            switch(setting)
            {
                case "de-DE":
                    return LocalisationHelper.GetString("Preferences_Language_German");
                case "en-GB":
                default:
                    return LocalisationHelper.GetString("Preferences_Language_English");
            };
        }

        public PreferencesViewModel()
        {
            AvailableLanguages = new List<string>
            {
                LocalisationHelper.GetString("Preferences_Language_German"),
                LocalisationHelper.GetString("Preferences_Language_English")
            };
        }

        internal void ApplySettings()
        {
            ApplyLanguage();
        }

        private void ApplyLanguage()
        {
            if (selectedLanguage.Equals(GetLanguageFromSettings())) return;

            if (selectedLanguage.Equals(LocalisationHelper.GetString("Preferences_Language_German")))
            {
                Properties.Settings.Default.UILanguage = "de-DE";
            }
            else
            {
                Properties.Settings.Default.UILanguage = "en-GB";
            }

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();

            log.Info("Applied application language " + Properties.Settings.Default.UILanguage);
        }
    }
}
