using DynamicData;
using DynamicData.Binding;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.UpdateLog;
using Prism.Mvvm;
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
    public class MainWindowModel : BindableBase
    {
        public ObservableCollectionExtended<MaxExportedCable> DataIn => _dataIn;
        public ReadOnlyObservableCollection<IDisplayableData> DataOut => new ReadOnlyObservableCollection<IDisplayableData>(_dataOut);
        public IUpdateLogger Logger => _converter.Logger;
        public bool IsUpdateFrozen { get; set; } = false;

        private readonly ObservableCollectionExtended<MaxExportedCable> _dataIn;
        private readonly ObservableCollectionExtended<IDisplayableData> _dataOut;
        private readonly Dispatcher _dispatcher;
        private DataConverter _converter;
        private Task<List<IDisplayableData>> _convertTask;

        public MainWindowModel(DataConverter converter)
        {
            _dataIn = new ObservableCollectionExtended<MaxExportedCable>();

            _dataOut = new ObservableCollectionExtended<IDisplayableData>();
            //DynamicData.Binding.ObservableCollectionAdaptor
            //DataOut = new ReadOnlyObservableCollection<IDisplayableData>(_dataOut);
            //_dataOut.Connect().To
            _converter = converter;
            converter.SettingsChanged += () => UpdateDataOut();

            _dispatcher = Dispatcher.CurrentDispatcher;
            //Dispatcher disp = Dispatcher.CurrentDispatcher;
            //_deferredUpdate = new DeferredOperation(() => disp.BeginInvoke(UpdateDataOut), 5);
        }

        public void SetDataIn(List<string[]> values)
        {
            #region Debug
#if DEBUG
            Stopwatch swSetDataIn = Stopwatch.StartNew();
#endif
            #endregion
            //IsUpdateFrozen = true;
            var suspend = _dataIn.SuspendNotifications();

            _dataIn.Clear();

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
                _dataIn.Add(cable);
            }

            suspend.Dispose();
            //IsUpdateFrozen = false;
            #region Debug
#if DEBUG
            Debug.WriteLine($"SetDataIn time - {swSetDataIn.ElapsedMilliseconds} ms");
#endif
            #endregion
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

        public async void UpdateDataOut()
        {
            #region Debug
#if DEBUG
            Debug.WriteLine($"Start UpdateDataOut - {DateTime.Now}");
#endif
            #endregion

            if (IsUpdateFrozen)
                return;

            #region Debug
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            #endregion

            Task<List<IDisplayableData>> currentTask = new Task<List<IDisplayableData>>(() => _converter.Convert(_dataIn.ToList()));
            _convertTask = currentTask;
            currentTask.Start();

            //            #region Debug
            //#if DEBUG
            //            Stopwatch swClear = Stopwatch.StartNew();
            //#endif
            //            #endregion

            //            #region Debug
            //#if DEBUG
            //            Debug.WriteLine($"Clear time - {swClear.ElapsedMilliseconds} ms");
            //#endif
            //            #endregion
            //var lst = _dataIn.ToList();
            List<IDisplayableData> data = await currentTask;//Task.Run(() => _converter.Convert(lst)); //new Task<List<IDisplayableData>>(() => _converter.Convert(_dataIn.ToList())).Start();

            //if (_convertTask != currentTask)
            //    return;
            _dispatcher.Invoke(() =>
            {
                using (_dataOut.SuspendNotifications())
                {
                    _dataOut.Clear();
                    _dataOut.AddRange(data);
                }
            });
            #region Debug
#if DEBUG
            Debug.WriteLine($"  Update time - {sw.ElapsedMilliseconds} ms");
#endif
            #endregion
        }
    }
}
