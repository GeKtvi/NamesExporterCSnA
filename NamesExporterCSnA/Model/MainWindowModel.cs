using GeKtviWpfToolkit;
using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Services.UpdateLog;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Windows.Threading;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel
    {
        public ObservableCollection<MaxExportedCable> DataIn { get; private set; }
        public ObservableCollection<IDisplayableData> DataOut { get; private set; }
        public IUpdateLogger Logger => _converter.Logger;
        public bool IsUpdateFrozen { get; set; } = false;

        private DataConverter _converter;
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
            IsUpdateFrozen = true;
            DataIn.Clear();

            foreach (string[] row in values)
            {
                int i = 0;
                MaxExportedCable cable = new MaxExportedCable();
                foreach (var cell in row)
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
            IsUpdateFrozen = false;
            UpdateDataOut();
        }

        public List<List<string>> GetDataAsListList()
        {
            List<List<string>> data = new();

            foreach (var itemDataOut in DataOut)
            {
                int i = 0;
                data.Add(new());
                foreach (PropertyDescriptor item in TypeDescriptor.GetProperties(DataOut.GetType().GetGenericArguments().Single()))
                {
                    PropertyDescriptor propertyDescriptor = item;
                    var attributes = propertyDescriptor.Attributes[typeof(System.ComponentModel.DataAnnotations.DisplayAttribute)];
                    System.ComponentModel.DataAnnotations.DisplayAttribute displayAttribute =
                            attributes as System.ComponentModel.DataAnnotations.DisplayAttribute;

                    if (displayAttribute != null && displayAttribute.GetAutoGenerateField() != false)
                        data.Last().Add(propertyDescriptor.GetValue(itemDataOut).ToString());
                    i++;
                }
            }

            return data;
        }

        public void UpdateDataOut()
        {
            if (IsUpdateFrozen)
                return;

            DataOut.Clear();

            List<IDisplayableData> dataOut = _converter.Convert(DataIn.ToList());
            foreach (var itemDataOut in dataOut)
                DataOut.Add(itemDataOut);
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
