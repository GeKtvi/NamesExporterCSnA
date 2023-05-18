using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using NamesExporterCSnA.ViewModel;

namespace NamesExporterCSnA.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindowView : Window
    {
        public MainWindowView(MainWindowViewModel viewModel)
        {
            InitializeComponentUiSave();
            DataContext = viewModel;
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

            //Properties.UI.Default.WindowSettings = Properties.UI.Default.WindowSettings;
            Properties.UI.Default.Save();
        }
    }
}
