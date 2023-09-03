using DynamicData;
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
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel : BindableBase
    {
        public SourceList<MaxExportedCable> DataIn => _dataIn;
        public SourceList<IDisplayableData> DataOut => _dataOut;
        public IUpdateLogger Logger => _converter.Logger;
        public bool IsUpdateFrozen { get; set; } = false;

        private readonly SourceList<MaxExportedCable> _dataIn;
        private readonly SourceList<IDisplayableData> _dataOut;
        private DataConverter _converter;
        private Task<List<IDisplayableData>> _convertTask;

        public MainWindowModel(DataConverter converter)
        {
            _dataIn = new SourceList<MaxExportedCable>();

            _dataOut = new SourceList<IDisplayableData>();
            //DataOut = new ReadOnlyObservableCollection<IDisplayableData>(_dataOut);

            _converter = converter;
            converter.SettingsChanged += UpdateDataOut;

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
            IsUpdateFrozen = true;

            _dataIn.Edit(_ =>
            {
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
            });
            IsUpdateFrozen = false;
            #region Debug
#if DEBUG
            Debug.WriteLine($"SetDataIn time - {swSetDataIn.ElapsedMilliseconds} ms");
#endif
            #endregion
        }

        public List<List<string>> GetDataAsListList()
        {
            List<List<string>> data = new();

            foreach (IDisplayableData itemDataOut in _dataOut.Items)
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

            Task<List<IDisplayableData>> currentTask = new Task<List<IDisplayableData>>(() => _converter.Convert(_dataIn.Items.ToList()));
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

            List<IDisplayableData> data = await _convertTask;

            if (_convertTask != currentTask)
                return;

            _dataOut.Edit(_ =>
            {
                _dataOut.Clear();
                _dataOut.AddRange(data);
            });
            
            #region Debug
#if DEBUG
            Debug.WriteLine($"  Update time - {sw.ElapsedMilliseconds} ms");
#endif
            #endregion
        }
    }
}
