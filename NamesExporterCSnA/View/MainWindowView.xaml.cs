using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.ViewModel;
using System;
using System.Windows;

namespace NamesExporterCSnA.View
{
    public partial class MainWindowView : System.Windows.Window
    {
        private IServiceProvider _services;

        public MainWindowView(MainWindowViewModel mainWindowViewModel, IServiceProvider serviceProvider)
        {
            _services = serviceProvider;
            InitializeComponentUiSave();
            DataContext = mainWindowViewModel;
            InitializeComponent();
            this.Closed += MainWindowClosed;
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

        private async void ShowUpdateFails(object sender, RoutedEventArgs e)
        {
            var updateDialog = new UpdateFails();
            updateDialog.DataContext = (this.DataContext as MainWindowViewModel).Logger;
            var result = await updateDialog.ShowAsync().ConfigureAwait(true);
        }

        private void ShowSettings(object sender, RoutedEventArgs e)
        {
            _services.GetRequiredService<SettingsWindowView>().Show();
        }
    }
}
