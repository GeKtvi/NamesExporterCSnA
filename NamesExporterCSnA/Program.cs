using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using NamesExporterCSnA.Model;
using NamesExporterCSnA.View;
using NamesExporterCSnA.ViewModel;

namespace NamesExporterCSnA
{
    public class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            App app = new App();
            MainWindowModel model = null;
            MainWindowViewModel viewModel = null;
            MainWindowView view = null;
            try
            {
                app.InitializeComponent();

                model = new MainWindowModel();
                viewModel = new MainWindowViewModel(model);
                view = new MainWindowView(viewModel);

                app.Run(view);

            }
            catch (Exception)
            {
                view?.Close();
                app?.Shutdown();

#if !DEBUG
                MessageBox.Show(new Window(),"Не обрабатываемая ошибка выполнения программы", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
#else
                throw;
#endif
            }

        }
    }
}
