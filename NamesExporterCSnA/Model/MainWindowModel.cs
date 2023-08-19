using GeKtviWpfToolkit;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.UpdateLog;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel : BindableBase
    {
        public ObservableCollection<MaxExportedCable> DataIn { get; private set; }
        public ObservableCollection<IDisplayableData> DataOut { get; private set; }
        public IUpdateLogger Logger => _converter.Logger;
        public bool IsUpdateFrozen { get; set; } = false;

        private DataConverter _converter;
        private Task<List<IDisplayableData>> _convertTask;
        private DeferredOperation _deferredUpdate; //нужно только для прямой вставки из DataGrid

        public MainWindowModel(DataConverter converter)
        {
            DataIn = new ObservableCollection<MaxExportedCable>();
            DataOut = new ObservableCollection<IDisplayableData>();
            _converter = converter;
            DataIn.CollectionChanged += DataInChanged;
            converter.SettingsChanged += UpdateDataOut;

            Dispatcher disp = Dispatcher.CurrentDispatcher;
            _deferredUpdate = new DeferredOperation(() => disp.BeginInvoke(UpdateDataOut), 5);
        }

        public void SetDataIn(List<string[]> values)
        {
            #region Debug
#if DEBUG
            Stopwatch swSetDataIn = Stopwatch.StartNew();
#endif
            #endregion
            IsUpdateFrozen = true;

            ObservableCollection<MaxExportedCable> dataIn = new ObservableCollection<MaxExportedCable>();
            dataIn.CollectionChanged += DataInChanged;

            dataIn.Clear();

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
                dataIn.Add(cable);
            }
            IsUpdateFrozen = false;
            DataIn = dataIn;
            #region Debug
#if DEBUG
            Debug.WriteLine($"SetDataIn time - {swSetDataIn.ElapsedMilliseconds} ms");
#endif
            #endregion

            UpdateDataOut();
        }

        public List<List<string>> GetDataAsListList()
        {
            List<List<string>> data = new();

            foreach (IDisplayableData itemDataOut in DataOut)
            {
                int i = 0;
                data.Add(new());
                foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(DataOut.GetType().GetGenericArguments().Single()))
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
            if (IsUpdateFrozen)
                return;

            #region Debug
#if DEBUG
            Stopwatch sw = Stopwatch.StartNew();
#endif
            #endregion

            Task<List<IDisplayableData>> currentTask = new Task<List<IDisplayableData>>(() => _converter.Convert(DataIn.ToList()));
            _convertTask = currentTask;
            currentTask.Start();

            #region Debug
#if DEBUG
            Stopwatch swClear = Stopwatch.StartNew();
#endif
            #endregion
            DataOut.Clear();
            #region Debug
#if DEBUG
            Debug.WriteLine($"Clear time - {swClear.ElapsedMilliseconds} ms");
#endif
            #endregion

            List<IDisplayableData> data = await _convertTask;

            if (_convertTask != currentTask)
                return;

            foreach (IDisplayableData itemDataOut in data)
                DataOut.Add(itemDataOut);

            #region Debug
#if DEBUG
            Debug.WriteLine($"Update time - {sw.ElapsedMilliseconds} ms");
#endif
            #endregion
        }

        private void DataInChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (IsUpdateFrozen == false)
                _deferredUpdate.DoOperation();

            if (e.NewItems == null)
                return;

            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += (s, e) => _deferredUpdate.DoOperation();
        }
    }
}
