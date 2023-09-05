using DynamicData;
using DynamicData.Binding;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.UpdateLog;
using NamesExporterCSnA.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Windows.Input;

namespace NamesExporterCSnA.ViewModel
{
    public class MainWindowViewModel : ReactiveObject
    {
        [Reactive]
        public ObservableCollectionExtended<MaxExportedCable> DataIn => _mainWindowModel.DataIn;

        [Reactive]
        public ReadOnlyObservableCollection<IDisplayableData> DataOut => _mainWindowModel.DataOut;

        [Reactive]
        public IUpdateLogger Logger { get => _mainWindowModel.Logger; }

        public IReactiveCommand ImportData { get; private set; }
        public IReactiveCommand ExportData { get; private set; }
        public IReactiveCommand ClearData { get; private set; }
        public ReactiveCommand<Unit, Unit> UpdateDataOut { get; private set; }

        private MainWindowModel _mainWindowModel;

        public MainWindowViewModel(MainWindowModel mainWindowModel)
        {
            _mainWindowModel = mainWindowModel;

            UpdateDataOut = ReactiveCommand.CreateFromObservable(() => Observable.Start(() => _mainWindowModel.UpdateDataOut()));
            UpdateDataOut.ThrownExceptions.Subscribe(e => throw e.InnerException);

            ImportData = ReactiveCommand.Create(
                SetTextFromClipboard,
                UpdateDataOut.IsExecuting.Select(x => x == false),
                RxApp.MainThreadScheduler);

            ExportData = ReactiveCommand.Create(
                SetTextToClipboard,
                _mainWindowModel.DataOut.ToObservableChangeSet().Throttle(TimeSpan.FromMilliseconds(25), DispatcherScheduler.Current).Select(data => data != null && data.Count != 0)
            );

            ClearData = ReactiveCommand.Create(
                ClearDataIn,
                _mainWindowModel.DataIn.ToObservableChangeSet().Select(data => data != null && data.Count != 0)
            );

            //_updateDataOut = ReactiveCommand.Create(() => _mainWindowModel.UpdateDataOut(), outputScheduler: DispatcherScheduler.Current);


            _mainWindowModel.DataIn.ToObservableChangeSet()
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Throttle(TimeSpan.FromMilliseconds(25))
                .AutoRefresh()
                //.Select(number => Observable.Defer(() => _mainWindowModel.UpdateDataOut().ToObservable()))
                //.Select(number => Observable.FromAsync(async () => await _mainWindowModel.UpdateDataOut()))
                //.Select(number => Observable.FromAsync(async () => await disp.BeginInvoke(() => _mainWindowModel.UpdateDataOut())))
                //.Select(number => Observable.Defer(() => Observable.Start(() => disp.BeginInvoke(() => _mainWindowModel.UpdateDataOut()))))
                //.Select(l => Observable.FromAsync(_ => _mainWindowModel.UpdateDataOut()))
                //.Concat()
                //.Select(x => Unit.Default)
                //.Select(x => Unit.Default)
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut)
                //.Subscribe()
                ;
        }

        private void SetTextFromClipboard()
        {
            List<string[]> data = ClipboardHelper.ParseClipboardData();
            if (data != null)
                _mainWindowModel.SetDataIn(data);
        }

        private void SetTextToClipboard()
        {
            ClipboardHelper.SetClipboardData(_mainWindowModel.GetDataAsListList());
        }

        private void ClearDataIn()
        {
            _mainWindowModel.DataIn.Clear();
        }
    }
}
