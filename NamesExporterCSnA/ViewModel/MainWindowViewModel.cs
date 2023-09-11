using DynamicData;
using DynamicData.Binding;
using NamesExporterCSnA.Data;
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
        public bool IsUpdateExecuting { get; private set; }

        public UpdateLoggerViewModel Logger { get; }

        public IReactiveCommand ImportData { get; private set; }
        public IReactiveCommand ExportData { get; private set; }
        public IReactiveCommand ClearData { get; private set; }
        public ReactiveCommand<Unit, Unit> UpdateDataOut { get; private set; }

        private MainWindowModel _mainWindowModel;

        public MainWindowViewModel(MainWindowModel mainWindowModel, UpdateLoggerViewModel updateLoggerViewModel)
        {
            _mainWindowModel = mainWindowModel;
            Logger = updateLoggerViewModel;

            UpdateDataOut = ReactiveCommand.CreateRunInBackground(() => _mainWindowModel.UpdateDataOut());
            UpdateDataOut.ThrownExceptions.Subscribe(e => throw e);
            UpdateDataOut.IsExecuting.BindTo(this, x => x.IsUpdateExecuting);

            ImportData = ReactiveCommand.Create(
                _mainWindowModel.SetTextFromClipboard,
                UpdateDataOut.IsExecuting.Select(x => x == false),
                RxApp.MainThreadScheduler);
            ImportData.ThrownExceptions.Subscribe(e => throw e);

            ExportData = ReactiveCommand.Create(
                _mainWindowModel.SetDataOutToClipboard,
                _mainWindowModel.DataOut.ToObservableChangeSet().Select(data => data != null && data.Count != 0)
            );
            ExportData.ThrownExceptions.Subscribe(e => throw e);

            ClearData = ReactiveCommand.Create(
                _mainWindowModel.DataIn.Clear,
                _mainWindowModel.DataIn.ToObservableChangeSet().Select(data => data != null && data.Count != 0)
            );
            ClearData.ThrownExceptions.Subscribe(e => throw e);

            _mainWindowModel.DataIn.ToObservableChangeSet()
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Throttle(TimeSpan.FromMilliseconds(25))
                .AutoRefresh()
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut);

            _mainWindowModel.SettingsChanging
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut);

            IDisposable updateDataOutSubscribe = UpdateDataOut.Subscribe();
            _mainWindowModel.SettingsChanging
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .Where(_ => IsUpdateExecuting)
                .Subscribe(_ =>
                    {
                        updateDataOutSubscribe?.Dispose();
                        updateDataOutSubscribe = UpdateDataOut.FirstAsync().Subscribe(_ => UpdateDataOut.Execute());
                    }
                );
        }
    }
}
