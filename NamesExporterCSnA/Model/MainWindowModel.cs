using DynamicData;
using DynamicData.Binding;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel
    {
        public ObservableCollectionExtended<MaxExportedCable> DataIn { get; }
        public ReadOnlyObservableCollection<IDisplayableData> DataOut => new ReadOnlyObservableCollection<IDisplayableData>(_dataOut);
        public IPreferencesSettings Settings => _converter.Settings;
        public IObservable<IPreferencesSettings> SettingsChanging =>
            _converter.Settings.WhenAnyPropertyChanged();

        private readonly ObservableCollectionExtended<IDisplayableData> _dataOut;
        private readonly Dispatcher _dispatcher;
        private DataConverter _converter;
        private object _updateDataOutRunningLock = new();
        private CancellationTokenSource _tokenSourceUpdateDataOut;
        private Task _updateDataOutTask;

        public MainWindowModel(DataConverter converter)
        {
            DataIn = new ObservableCollectionExtended<MaxExportedCable>();
            _dataOut = new ObservableCollectionExtended<IDisplayableData>();

            _converter = converter;

            _dispatcher = Dispatcher.CurrentDispatcher;
        }

        public void SetDataIn(List<string[]> values)
        {
            #region Debug
#if DEBUG
            Stopwatch swSetDataIn = Stopwatch.StartNew();
#endif
            #endregion
            IDisposable suspend = DataIn.SuspendNotifications();

            DataIn.Clear();

            foreach (string[] row in values)
            {
                int i = 0;
                MaxExportedCable cable = new MaxExportedCable();
                foreach (string cell in row)
                {
                    switch (i)
                    {
                        case 0:
                            cable.SchemeName = cell;
                            break;
                        case 1:
                            cable.WireName = cell;
                            break;
                        default:
                            break;
                    }
                    i++;
                }
                DataIn.Add(cable);
            }

            suspend.Dispose();
            #region Debug
#if DEBUG
            Debug.WriteLine($"SetDataIn time - {swSetDataIn.ElapsedMilliseconds} ms");
#endif
            #endregion
        }

        public void SetDataOutToClipboard()
        {
            try
            {
                ClipboardHelper.SetClipboardData(GetDataAsListList());
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                if (e.HResult != -2147221040)
                    throw;
            }
        }

        public void SetTextFromClipboard()
        {
            List<string[]> data = null;
            try
            {
                data = ClipboardHelper.ParseClipboardData();
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                if (e.HResult != -2147221040)
                    throw;
            }
            SetDataIn(data ?? new List<string[]>());
        }

        public List<List<string>> GetDataAsListList()
        {
            List<List<string>> data = new();

            foreach (IDisplayableData itemDataOut in _dataOut)
            {
                int i = 0;
                data.Add(new());
                foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(_dataOut.GetType().GetGenericArguments().Single()))
                {
                    PropertyDescriptor propertyDescriptor = item;
                    System.Attribute attributes = propertyDescriptor.Attributes[typeof(System.ComponentModel.DataAnnotations.DisplayAttribute)];

                    if (attributes is System.ComponentModel.DataAnnotations.DisplayAttribute displayAttribute && displayAttribute.GetAutoGenerateField() != false)
                        data.Last().Add(propertyDescriptor.GetValue(itemDataOut).ToString());
                    i++;
                }
            }

            return data;
        }

        public async void RunUpdateDataOut()
        {
            lock (_updateDataOutRunningLock)
            {
                if (_updateDataOutTask is not null && 
                    _tokenSourceUpdateDataOut is not null && 
                    _updateDataOutTask.Status == TaskStatus.Running)
                {
                    _tokenSourceUpdateDataOut.Cancel();
                    #region Debug
#if DEBUG
                    Debug.WriteLine($"UpdateDataOut start cancel - {DateTime.Now}");
#endif
                    #endregion
                    return;
                }

                _tokenSourceUpdateDataOut = new CancellationTokenSource();
                CancellationToken token = _tokenSourceUpdateDataOut.Token;

                _updateDataOutTask = Task.Run(() => UpdateDataOut(token), token);
            }

            bool lockWasTaken = false;
            try
            {
                await _updateDataOutTask;
                Monitor.Enter(_updateDataOutRunningLock, ref lockWasTaken);
            }
            catch (AggregateException ae)
            {
                if (!ae.InnerExceptions.Select(x => x is OperationCanceledException).Any())
                    throw ae;
            }
            catch (OperationCanceledException) { }
            finally
            {
                _tokenSourceUpdateDataOut?.Dispose();
                _tokenSourceUpdateDataOut = null;
                if (lockWasTaken)
                    Monitor.Exit(_updateDataOutRunningLock);
            }

            if (_updateDataOutTask.IsCanceled)
            {
                #region Debug
#if DEBUG
                Debug.WriteLine($"Start re invoke UpdateDataOut - {DateTime.Now}");
#endif
                #endregion
                RunUpdateDataOut();
            }
        }

        public void UpdateDataOut(CancellationToken token)
        {
            #region Debug
#if DEBUG
            Debug.WriteLine($"Start UpdateDataOut - {DateTime.Now}");
            Stopwatch sw = Stopwatch.StartNew();
#endif
            #endregion
            token.ThrowIfCancellationRequested();
            List<MaxExportedCable> dataIn = null;

            //Starts copying on the dispatcher thread to avoid changes during copying
            _dispatcher.Invoke(() => dataIn = DataIn.ToList(), DispatcherPriority.Normal, token);

            token.ThrowIfCancellationRequested();
             
            List<IDisplayableData> data = _converter.Convert(dataIn, token);
            token.ThrowIfCancellationRequested();
            _dispatcher.Invoke(() =>
            {
                #region Debug
#if DEBUG
                Stopwatch sw = Stopwatch.StartNew();
#endif
                #endregion
                using (_dataOut.SuspendNotifications())
                {
                    _dataOut.Clear();
                    _dataOut.AddRange(data);
                }
                #region Debug
#if DEBUG
                Debug.WriteLine($"  Set time - {sw.ElapsedMilliseconds} ms");
#endif
                #endregion
            }, DispatcherPriority.Normal, token);
            #region Debug
#if DEBUG
            Debug.WriteLine($"  Update time - {sw.ElapsedMilliseconds} ms");
#endif
            #endregion
        }
    }
}
