using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.ViewModel;
using System;
using System.Windows;

namespace NamesExporterCSnA.View
{
    public partial class MainWindowView : Window
    {
        private IServiceProvider _services;
        private IServiceScope _settingsServiceScope;

        public MainWindowView(MainWindowViewModel mainWindowViewModel, IServiceProvider serviceProvider)
        {
            _services = serviceProvider;
            InitializeComponentUiSave();
            DataContext = mainWindowViewModel;
            InitializeComponent();
            Closed += MainWindowClosed;
        }

        private void InitializeComponentUiSave()
        {
            if (Properties.UI.Default.MainWindowSettings == null)
                Properties.UI.Default.MainWindowSettings = new WindowSettings();
            if (Properties.UI.Default.MainWindowSettings.WindowState == WindowState.Minimized)
                Properties.UI.Default.MainWindowSettings.WindowState = WindowState.Normal;
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            Properties.UI.Default.Save();
        }

        private async void ShowUpdateFails(object sender, RoutedEventArgs e) //TODO create command
        {
            UpdateFails updateDialog = new();
            updateDialog.DataContext = (DataContext as MainWindowViewModel).Logger;
            await updateDialog.ShowAsync().ConfigureAwait(true);
        }

        private void ShowSettings(object sender, RoutedEventArgs e) //TODO create command
        {
            if (_settingsServiceScope is null)
                _settingsServiceScope = _services.CreateScope();
            SettingsWindowView window = _settingsServiceScope.ServiceProvider.GetRequiredService<SettingsWindowView>();
            window.Closed += (s, e) =>
            {
                _settingsServiceScope?.Dispose();
                _settingsServiceScope = null;
            };
            window.Owner = this;
            window.Show();
        }
    }
}
