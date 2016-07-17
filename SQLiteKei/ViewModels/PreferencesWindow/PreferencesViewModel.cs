using log4net;

using SQLiteKei.Helpers;

using System.Collections.Generic;
using System;

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

        public List<string> AvailableThemes { get; set; }

        private string selectedTheme;
        public string SelectedTheme
        {
            get
            {
                if (string.IsNullOrEmpty(selectedTheme))
                    selectedTheme = Properties.Settings.Default.UITheme;

                return selectedTheme;
            }
            set { selectedTheme = value; }
        }

        public PreferencesViewModel()
        {
            AvailableLanguages = new List<string>
            {
                LocalisationHelper.GetString("Preferences_Language_German"),
                LocalisationHelper.GetString("Preferences_Language_English")
            };

            AvailableThemes = new List<string>
            {
                "Dark", "Light"
            };
        }

        internal void ApplySettings()
        {
            ApplyLanguage();
            ApplyApplicationTheme();

            Properties.Settings.Default.Save();
            Properties.Settings.Default.Reload();
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

            log.Info("Applied application language " + Properties.Settings.Default.UILanguage);
        }

        private void ApplyApplicationTheme()
        {
            if (selectedTheme.Equals(Properties.Settings.Default.UITheme)) return;

            log.Info("Applying '" + selectedTheme + "' application theme.");

            try
            {
                ThemeHelper.LoadTheme(selectedTheme);
                Properties.Settings.Default.UITheme = selectedTheme;
            }
            catch(Exception ex)
            {
                log.Error("Could not apply application theme '" + selectedTheme + "'.", ex);
            }
        }
    }
}
