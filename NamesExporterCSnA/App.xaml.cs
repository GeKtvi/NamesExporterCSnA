using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.Model;
using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Services.Settings;
using NamesExporterCSnA.Services.UpdateLog;
using NamesExporterCSnA.View;
using NamesExporterCSnA.ViewModel;
using System;
using System.Windows;

namespace NamesExporterCSnA
{
    public partial class App : Application
    {
        public static IServiceProvider CurrentServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

#if !DEBUG
            DispatcherUnhandledException += (s, e) =>
                MessageBox.Show($"Не обрабатываемая ошибка выполнения программы \n\n {e.Exception.Message}\n\n {e.Exception.StackTrace}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
#endif

            CurrentServiceProvider = new ServiceCollection()
                            .AddSingleton<MainWindowModel>()
                            .AddSingleton<MainWindowViewModel>()
                            .AddSingleton<MainWindowView>()
                            .AddSingleton<DataConverter>()
                            .AddSingleton<IUpdateLogger, UpdateLogger>()
                            .AddScoped<SettingsWindowModel>()
                            .AddScoped<SettingsWindowViewModel>()
                            .AddScoped<SettingsWindowView>()
                            .AddSingleton<IPreferencesSettings, PreferencesSettings>()
                            .AddSingleton<SettingsSaveLoadManager>()
                            .BuildServiceProvider();

            CurrentServiceProvider.GetRequiredService<SettingsSaveLoadManager>();

            MainWindow = CurrentServiceProvider.GetService<MainWindowView>();
            CurrentServiceProvider.GetRequiredService<MainWindowView>().Show();
        }
    }
}
