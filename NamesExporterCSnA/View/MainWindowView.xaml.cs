using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.Data.UpdateLog;
using NamesExporterCSnA.ViewModel;
using Prism.Commands;
using System;
using System.Windows;
using System.Windows.Input;
using Wpf.Ui.Controls;

namespace NamesExporterCSnA.View
{
    public partial class MainWindowView
    {
        public ICommand ChangeUpdateFailsVisibility { get; private set; }
        public ICommand ShowSettings { get; private set; }

        private IServiceProvider _services;
        private IServiceScope _settingsServiceScope;

        public MainWindowView(MainWindowViewModel mainWindowViewModel, IServiceProvider serviceProvider)
        {
            Wpf.Ui.Appearance.Watcher.Watch(this);

            ChangeUpdateFailsVisibility = new DelegateCommand(ChangeUpdateFailsDialogVisibility);
            ShowSettings = new DelegateCommand(ShowSettingsWindow);

            _services = serviceProvider;
            InitializeComponentUiSave();
            DataContext = mainWindowViewModel;
            InitializeComponent();

            Closed += MainWindowClosed;
            Dialog.ButtonRightClick += (s, e) => Dialog.Hide();
            ContentRendered
                += (s, e) => base.OnStateChanged(new EventArgs());
            DragOver += (s, e) => Activate();
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

        private void ChangeUpdateFailsDialogVisibility()
        {
            if (((MainWindowViewModel)DataContext).Logger.Status == LoggerStatus.NoFails)
                return;

            if (Dialog.IsShown)
            {
                Dialog.Hide();
                return;
            }

            Dialog updateDialog = Dialog;
            updateDialog.DataContext = (DataContext as MainWindowViewModel).Logger;
            updateDialog.Show();
        }

        private void ShowSettingsWindow()
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
