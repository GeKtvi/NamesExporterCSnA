﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NamesExporterCSnA.Model;
using NamesExporterCSnA.Model.Data;
using Prism.Mvvm;
using Prism.Commands;
using System.Windows.Input;
using GeKtviWpfToolkit;
using System.Windows;
using System.ComponentModel;
using GeKtviWpfToolkit.Controls;

namespace NamesExporterCSnA.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<MaxExportedCable> DataIn 
        { 
            get => _mainWindowModel.DataIn; 
            set => _mainWindowModel.DataIn = value; 
        }
        public ObservableCollection<object> DataOut 
        { 
            get => _mainWindowModel.DataOut; 
            set => _mainWindowModel.DataOut = value; 
        }

        public ICommand ImportData { get; private set; }
        public ICommand ExportData { get; private set; }

        private MainWindowModel _mainWindowModel { get; set; }

        public MainWindowViewModel(MainWindowModel model)
        {
            _mainWindowModel = model;

            ImportData = new DelegateCommand(SetTextFromClipboard);
            ExportData = new DelegateCommand(SetTextToClipboard, CanExecuteExportData);

            DataOut.CollectionChanged += DataOutCollectionChanged;
        }

        private void DataOutCollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            (ExportData as DelegateCommand).RaiseCanExecuteChanged();
        }

        private void SetTextFromClipboard()
        {
            var data = ClipboardHelper.ParseClipboardData();
            if (data != null)
                _mainWindowModel.SetDataIn(data);
        }

        private void SetTextToClipboard()
        {
            //DataObject data = new DataObject("sdfadsgf");
            //Clipboard.SetDataObject(data);
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
                    //item.SetValue(cable, cell);
                    i++;
                }
            }
            ClipboardHelper.SetClipboardData(data);
        }

        private bool CanExecuteExportData()
        {
            return DataOut != null && DataOut.Count() != 0;
        }
    }
}
