using log4net;

using SQLiteKei.Helpers;

using System;
using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace SQLiteKei
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private ILog log = LogHelper.GetLogger();

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            log.Fatal("An unhandled exception was thrown and a message is shown to the user.", e.Exception);

            var message = LocalisationHelper.GetString("UnhandledException");
            var title = LocalisationHelper.GetString("UnhandledException_Title");

            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
            e.Handled = true;

            Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SQLiteKei.Properties.Settings.Default.UILanguage);
            base.OnStartup(e);

            log.Info("============= Application Startup ==============");
            log.Info("Running on " + Environment.OSVersion);
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            log.Info("============= Application Shutdown =============");
        }
    }
}
