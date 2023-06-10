using NamesExporterCSnA.ViewModel;
using System;
using System.Windows;

namespace NamesExporterCSnA.View
{
    public partial class MainWindowView : Window
    {
        public MainWindowView(MainWindowViewModel mainWindowViewModel)
        {
            InitializeComponentUiSave();
            DataContext = mainWindowViewModel;
            InitializeComponent();
            this.Closed += MainWindowClosed;
        }

        private void InitializeComponentUiSave()
        {
            if (Properties.UI.Default.WindowSettings == null)
                Properties.UI.Default.WindowSettings = new WindowSettings();
            if (Properties.UI.Default.WindowSettings.WindowState == WindowState.Minimized)
                Properties.UI.Default.WindowSettings.WindowState = WindowState.Normal;
        }

        private void MainWindowClosed(object sender, EventArgs e)
        {
            Properties.UI.Default.Save();
        }
    }
}
