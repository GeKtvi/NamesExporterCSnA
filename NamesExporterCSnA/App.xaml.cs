using GeKtviWpfToolkit;
using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.Cables;
using NamesExporterCSnA.Data.Marks;
using NamesExporterCSnA.Data.Marks.Exceptions;
using NamesExporterCSnA.Data.Settings;
using NamesExporterCSnA.Data.UpdateLog;
using NamesExporterCSnA.Model;
using NamesExporterCSnA.Services;
using NamesExporterCSnA.View;
using NamesExporterCSnA.ViewModel;
using ReactiveUI;
using System;
using System.Reactive.Concurrency;
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
                            .AddSingleton(AppConfigHelper.LoadConfig<CablesParserConfig>("CablesParser.config"))
                            .AddSingleton(AppConfigHelper.LoadConfig<CableMarkVendorData[]>("CableMarks.config"))
                            .AddSingleton(AppConfigHelper.LoadConfig<CableMarkingWhiteList>("CableForMarkingWhiteList.config"))
                            .BuildServiceProvider();

            CurrentServiceProvider.GetRequiredService<SettingsSaveLoadManager>();

            MainWindow = CurrentServiceProvider.GetService<MainWindowView>();
            CurrentServiceProvider.GetRequiredService<MainWindowView>().Show();
        }
    }
}
