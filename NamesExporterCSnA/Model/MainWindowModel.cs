using NamesExporterCSnA.Model;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Model.Data.Marks;
using System.Windows.Threading;
using System.Threading;
using System.Windows;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel : BindableBase
    {
        public ObservableCollection<MaxExportedCable> DataIn { get; private set; }
        public ObservableCollection<object> DataOut { get; private set; }

        public bool IsUpdateFeezed { get; set; } = false;

        private CablesParser _cablesParser = new();
        private CableMarkFabric _cableMarkDKCFabric = new CableMarkFabric();

        private DeferredOperation _deferredUpdate;
        private Thread _notificationThread; // заглушка

        public MainWindowModel()
        {
            DataIn = new ObservableCollection<MaxExportedCable>();
            DataOut = new ObservableCollection<object>();
            DataIn.CollectionChanged += DataInChanged;
            Dispatcher disp = Dispatcher.CurrentDispatcher;
            _deferredUpdate = new DeferredOperation(() => disp.BeginInvoke(UpdateDataOut), 500);
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

        private void DataInChanged(object? sender, NotifyCollectionChangedEventArgs e)
        {
            _deferredUpdate.DoOperation();

            if (e.NewItems == null)
                return;

            foreach (INotifyPropertyChanged item in e.NewItems)
                item.PropertyChanged += (s, e) => _deferredUpdate.DoOperation();

        }

        private void UpdateDataOut()
        {
            if (IsUpdateFeezed)
                return;

            DataOut.Clear();

            try
            {
                var parsed = _cablesParser.Parse(DataIn.ToList());

                if (parsed.Count == 0)
                    return;

                List<ICableMark> marks = new();

                foreach (var cable in parsed)
                    marks.AddRange(_cableMarkDKCFabric.GetMarksByCableName(cable));

                SetDataOutFromListICableMark(marks);
            }
            catch (Exception e)
            {
                //заглушка
                if (_notificationThread == null || _notificationThread.ThreadState != ThreadState.Running)
                {
                    _notificationThread = new Thread(() =>
                    MessageBox.Show("Ошибка при генерации списка:\n" + e.Message, 
                    "Ошибка обновления", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Warning, 
                    MessageBoxResult.OK)
                    );
                    _notificationThread.Start();
                }
                //заглушка
            }
        }

        private void SetDataOutFromListICableMark(List<ICableMark> marks)
        {
            var groupedMarks =
            from mark in marks
            group mark by mark.FullName into newGroup
            orderby newGroup.Key
            select newGroup;

            foreach (var item in groupedMarks)
                DataOut.Add(new DisplayableMark(item));
        }
    }
}
