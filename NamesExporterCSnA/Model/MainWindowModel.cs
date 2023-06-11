﻿using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel
    {
        public ObservableCollection<MaxExportedCable> DataIn { get; private set; }
        public ObservableCollection<object> DataOut { get; private set; }
        public string SelectedCableMarkVendor { get => _converter.CableMarkDKCFabric.SelectedVendorName; set => _converter.CableMarkDKCFabric.SelectedVendorName = value; }
        public ReadOnlyCollection<string> CableMarksVendors { get => _converter.CableMarkDKCFabric.VendorsNames; }
        public IUpdateLogger Logger => _converter.Logger;
        public bool IsUpdateFeezed { get; set; } = false;

        private DataConverter _converter;

        public MainWindowModel(DataConverter converter)
        {
            DataIn = new ObservableCollection<MaxExportedCable>();
            DataOut = new ObservableCollection<object>();
            _converter = converter;
            DataIn.CollectionChanged += DataInChanged;
        }

        public void SetDataIn(List<string[]> values)
        {
            IsUpdateFeezed = true;
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
            IsUpdateFeezed = false;
            UpdateDataOut();
        }

        public List<List<string>> GetDataAsListList()
        {
            List<List<string>> data = new();

            foreach (var itemDataOut in DataOut)
            {
                int i = 0;
                data.Add(new());
                foreach (var item in TypeDescriptor.GetProperties(DataOut.First()))
                {
                    System.ComponentModel.PropertyDescriptor propertyDescriptor = item as System.ComponentModel.PropertyDescriptor;
                    var attributes = propertyDescriptor.Attributes[typeof(System.ComponentModel.DataAnnotations.DisplayAttribute)];
                    System.ComponentModel.DataAnnotations.DisplayAttribute displayAttribute =
                            attributes as System.ComponentModel.DataAnnotations.DisplayAttribute;

                    if (displayAttribute.GetAutoGenerateField() != false)
                        data.Last().Add(propertyDescriptor.GetValue(itemDataOut).ToString());
                    i++;
                }
            }

            return data;
        }

        public void UpdateDataOut()
        {
            if (IsUpdateFeezed)
                return;

            DataOut.Clear();

                List<DisplayableDataOut> dataOut = _converter.Convert(DataIn.ToList());
                foreach (var itemDataOut in dataOut)
                    DataOut.Add(itemDataOut);
        }

        private void DataInChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            UpdateDataOut();

            if (e.NewItems == null)
                return;

            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += (s, e) => UpdateDataOut(); 

        }
    }
}
