using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.Model;
using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Services.UpdateLog;
using NamesExporterCSnA.View;
using NamesExporterCSnA.ViewModel;
using NamesExporterCSnA.Services.Settings;
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

            try
            {
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

                CurrentServiceProvider.GetRequiredService<SettingsSaveLoadManager>(); //Иначе инстанс не создастся

                MainWindow = CurrentServiceProvider.GetService<MainWindowView>();  
                CurrentServiceProvider.GetRequiredService<MainWindowView>().Show();
            }
            catch (Exception)
            {
#if !DEBUG
                MessageBox.Show(new Window(),"Не обрабатываемая ошибка выполнения программы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
#else
                throw;
#endif
            }
        }
    }
}
