using log4net;

using SQLiteKei.Helpers;
using SQLiteKei.Views;

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

            new UnhandledExceptionWindow().ShowDialog();
            e.Handled = true;

            Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(SQLiteKei.Properties.Settings.Default.UILanguage);
            base.OnStartup(e);

            string assemblyVersion = System.Reflection.Assembly.GetExecutingAssembly()
                                           .GetName()
                                           .Version
                                           .ToString();

            log.Info("================= GENERAL INFO =================");
            log.Info("SQLite Kei " + assemblyVersion);
            log.Info("Running on " + Environment.OSVersion);
            log.Info("Application language is " + Thread.CurrentThread.CurrentUICulture);
            log.Info("================================================");

            ThemeHelper.LoadCurrentUserTheme();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            base.OnExit(e);

            log.Info("============= APPLICATION SHUTDOWN =============");
        }
    }
}
