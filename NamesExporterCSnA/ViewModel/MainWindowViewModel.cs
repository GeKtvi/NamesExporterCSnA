using GeKtviWpfToolkit;
using NamesExporterCSnA.Model;
using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Services.UpdateLog;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace NamesExporterCSnA.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<MaxExportedCable> DataIn
        {
            get => _mainWindowModel.DataIn;
        }
        public ObservableCollection<IDisplayableData> DataOut
        {
            get => _mainWindowModel.DataOut;
        }

        public IUpdateLogger Logger { get => _mainWindowModel.Logger; }

        public ICommand ImportData { get; private set; }
        public ICommand ExportData { get; private set; }
        public ICommand ClearData { get; private set; }

        private MainWindowModel _mainWindowModel { get; set; }

        public MainWindowViewModel(MainWindowModel mainWindowModel)
        {
            _mainWindowModel = mainWindowModel;

            ImportData = new DelegateCommand(SetTextFromClipboard);
            ExportData = new DelegateCommand(SetTextToClipboard, CanExecuteExportData);
            ClearData = new DelegateCommand(ClearDataIn, CanExecuteClearDataIn);

            DataIn.CollectionChanged += DataInCollectionChanged;
            DataOut.CollectionChanged += DataOutCollectionChanged;

            _mainWindowModel.PropertyChanged += (s, e) => OnPropertyChanged(e);
            _mainWindowModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(DataIn) && DataIn != null)
                {
                    DataInCollectionChanged(s, null);
                    DataIn.CollectionChanged += DataInCollectionChanged;
                }
            };
            _mainWindowModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(DataOut) && DataOut != null)
                {
                    DataOutCollectionChanged(s, null);
                    DataOut.CollectionChanged += DataOutCollectionChanged;
                }
            };
        }

        private void DataInCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            (ClearData as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void DataOutCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            (ExportData as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void SetTextFromClipboard()
        {
            var data = ClipboardHelper.ParseClipboardData();
            if (data != null)
                _mainWindowModel.SetDataIn(data);
        }

        private void SetTextToClipboard()
        {
            ClipboardHelper.SetClipboardData(_mainWindowModel.GetDataAsListList());
        }

        private bool CanExecuteExportData()
        {
            return DataOut != null && DataOut.Count() != 0;
        }

        private void ClearDataIn()
        {
            _mainWindowModel.DataIn.Clear();
        }

        private bool CanExecuteClearDataIn()
        {
            return DataIn != null && DataIn.Count() != 0;
        }
    }
}
