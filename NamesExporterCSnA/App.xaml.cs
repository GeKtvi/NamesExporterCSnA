using NamesExporterCSnA.Model;
using NamesExporterCSnA.View;
using NamesExporterCSnA.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace NamesExporterCSnA
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            try
            {
                MainWindowModel model = null;
                MainWindowViewModel viewModel = null;
                MainWindowView view = null;

                model = new MainWindowModel();
                viewModel = new MainWindowViewModel(model);
                view = new MainWindowView(viewModel);

                view.Show();
            }
            catch (Exception)
            {
                //view?.Close();
                //app?.Shutdown();

#if !DEBUG
                    MessageBox.Show(new Window(),"Не обрабатываемая ошибка выполнения программы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
#else
                throw;
#endif
            }
        }
    }
}
