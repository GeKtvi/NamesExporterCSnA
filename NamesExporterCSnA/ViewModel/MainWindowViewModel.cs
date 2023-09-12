using DynamicData;
using DynamicData.Binding;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Model;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
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

            //Configure UpdateDataOut command
            //Binding IsExecuting to IsUpdateExecuting prop
            UpdateDataOut = ReactiveCommand.CreateRunInBackground(_mainWindowModel.UpdateDataOut);
            UpdateDataOut.ThrownExceptions.Subscribe(e => throw e);
            UpdateDataOut.IsExecuting.BindTo(this, x => x.IsUpdateExecuting);

            //Configure ImportData command
            //Can be executed when UpdateDataOut isn't executing
            ImportData = ReactiveCommand.Create(
                _mainWindowModel.SetTextFromClipboard,
                UpdateDataOut.IsExecuting.Select(x => x == false),
                RxApp.MainThreadScheduler);
            ImportData.ThrownExceptions.Subscribe(e => throw e);

            //Configure ExportData command
            //Can be executed when DataOut has elements
            ExportData = ReactiveCommand.Create(
                _mainWindowModel.SetDataOutToClipboard,
                _mainWindowModel.DataOut.ToObservableChangeSet()
                .Select(_ => DataOut != null && DataOut.Count != 0)
            );
            ExportData.ThrownExceptions.Subscribe(e => throw e);

            //Configure ClearData command
            //Can be executed when DataIn has elements and UpdateDataOut isn't executing
            ClearData = ReactiveCommand.Create(
                _mainWindowModel.DataIn.Clear,
                _mainWindowModel.DataIn.ToObservableChangeSet()
                .Select(_ => DataIn != null && DataIn.Count != 0)
                .CombineLatest(UpdateDataOut.IsExecuting, (canExecute, isUpdateDataOutExecuting) => canExecute == true && isUpdateDataOutExecuting == false)
            );
            ClearData.ThrownExceptions.Subscribe(e => throw e);

            //Configure invoke of UpdateDataOut command
            //Invokes when any property of DataIn items changed
            _mainWindowModel.DataIn.ToObservableChangeSet()
                .Throttle(TimeSpan.FromMilliseconds(25), RxApp.TaskpoolScheduler)
                .WhenAnyPropertyChanged()
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut);

            //Configure invoke of UpdateDataOut command
            //Invokes when collection DataIn changed
            _mainWindowModel.DataIn.ToObservableChangeSet()
                .Throttle(TimeSpan.FromMilliseconds(25), RxApp.TaskpoolScheduler)
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut);

            //Configure invoke of UpdateDataOut command
            //Invokes when settings in model changed
            _mainWindowModel.SettingsChanging
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                .Select(x => Unit.Default)
                .InvokeCommand(UpdateDataOut);

            //Configure deferred invoke of UpdateDataOut command
            //Invokes subscribes deferred invoke of UpdateDataOut
            IDisposable updateDataOutSubscribe = null;
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
