using DynamicData;
using DynamicData.Binding;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.UpdateLog;
using NamesExporterCSnA.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Reactive;
using System.Reactive.Linq;

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
                _mainWindowModel.SetTextFromClipboard,
                UpdateDataOut.IsExecuting.Select(x => x == false),
                RxApp.MainThreadScheduler);

            ExportData = ReactiveCommand.Create(
                () => ClipboardHelper.SetClipboardData(_mainWindowModel.GetDataAsListList()),
                _mainWindowModel.DataOut.ToObservableChangeSet().Select(data => data != null && data.Count != 0)
            );

            ClearData = ReactiveCommand.Create(
                _mainWindowModel.DataIn.Clear,
                _mainWindowModel.DataIn.ToObservableChangeSet().Select(data => data != null && data.Count != 0)
            );

            _mainWindowModel.DataIn.ToObservableChangeSet()
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Throttle(TimeSpan.FromMilliseconds(25))
                .AutoRefresh()
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut);
        }
    }
}
