using DynamicData;
using DynamicData.Binding;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.UpdateLog;
using NamesExporterCSnA.Model;
using Prism.Commands;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Reactive.Concurrency;
using System.Reactive.Linq;

namespace NamesExporterCSnA.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        [Reactive]
        public ObservableCollectionExtended<MaxExportedCable> DataIn { get; set; } = new ObservableCollectionExtended<MaxExportedCable>();

        [Reactive]
        public ObservableCollectionExtended<IDisplayableData> DataOut { get; set; } = new ObservableCollectionExtended<IDisplayableData>();

        [Reactive]
        public IUpdateLogger Logger { get => _mainWindowModel.Logger; }

        public IReactiveCommand ImportData { get; private set; }
        public IReactiveCommand ExportData { get; private set; }
        public IReactiveCommand ClearData { get; private set; }

        private MainWindowModel _mainWindowModel { get; set; }

        public MainWindowViewModel(MainWindowModel mainWindowModel)
        {
            _mainWindowModel = mainWindowModel;

            ImportData = ReactiveCommand.Create(SetTextFromClipboard, outputScheduler: RxApp.MainThreadScheduler);
            ExportData = ReactiveCommand.Create(
                SetTextToClipboard,
                this.WhenAnyValue(vm => vm.DataOut).Select(data => data != null && data.Count != 0)
            );

            ClearData = ReactiveCommand.Create(
                ClearDataIn,
                this.WhenAnyValue(vm => vm.DataIn).Select(data => data != null && data.Count != 0)
            );

            _mainWindowModel.DataIn.Connect()
                .ObserveOn(DispatcherScheduler.Current)
                .Bind(DataIn)
                .Subscribe(_ => Debug.Print("DataIn changed"));

            _mainWindowModel.DataOut.Connect()
                .ObserveOn(DispatcherScheduler.Current)
                .Bind(DataOut)
                .Subscribe(_ => Debug.Print("DataOut changed"));

            _mainWindowModel.DataIn.Connect()
                .AutoRefresh(scheduler:DispatcherScheduler.Current)
                .ObserveOn(DispatcherScheduler.Current)
                .Throttle(TimeSpan.FromMilliseconds(25), DispatcherScheduler.Current)
                .Subscribe(_ => _mainWindowModel.UpdateDataOut());
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
            return DataIn != null && DataIn.Count != 0;
        }
    }
}
