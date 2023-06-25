using GeKtviWpfToolkit;
using NamesExporterCSnA.ViewModel;
using System;
using System.Windows;

namespace NamesExporterCSnA.View
{
    /// <summary>
    /// Interaction logic for SettingsWindowView.xaml
    /// </summary>
    public partial class SettingsWindowView : Window
    {
        public SettingsWindowView(SettingsWindowViewModel viewModel)
        {
            DataContext = viewModel;
            InitializeComponent();
            InitializeComponentUiSave();
        }

        private void InitializeComponentUiSave()
        {
            if (Properties.UI.Default.SettingsWindowSettings == null)
            {
                WindowStartupLocation = WindowStartupLocation.CenterOwner;
                Properties.UI.Default.SettingsWindowSettings =
                        new DefaultWindowSettings()
                        {
                            Top = this.Top,
                            Left = this.Left,
                            Width = 500,
                            Height = 590,
                        };
            }
            if (Properties.UI.Default.SettingsWindowSettings.WindowState == WindowState.Minimized)
                Properties.UI.Default.SettingsWindowSettings.WindowState = WindowState.Normal;
        }

        private void SettingsWindowClosed(object sender, EventArgs e)
        {
            Properties.UI.Default.Save();
        }
    }
}
