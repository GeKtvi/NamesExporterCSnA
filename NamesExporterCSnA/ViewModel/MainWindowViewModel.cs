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
using System.Data;
using Microsoft.Extensions.DependencyInjection;
using NamesExporterCSnA.Services;

namespace NamesExporterCSnA.ViewModel
{
    public class MainWindowViewModel : BindableBase
    {
        public ObservableCollection<MaxExportedCable> DataIn
        {
            get => _mainWindowModel.DataIn;
        }
        public ObservableCollection<object> DataOut
        {
            get => _mainWindowModel.DataOut;
        }

        public string SelectedCableMarkVendor { 
            get => _mainWindowModel.SelectedCableMarkVendor;
            set
            {
                _mainWindowModel.SelectedCableMarkVendor = value;
                _mainWindowModel.UpdateDataOut();
            }
        }

        public ReadOnlyCollection<string> CableMarksVendors { get => _mainWindowModel.CableMarksVendors; }

        public IUpdateLogger Logger { get => _mainWindowModel.Logger; }

        public ICommand ImportData { get; private set; }
        public ICommand ExportData { get; private set; }
        public ICommand ClearData { get; private set; }

        private MainWindowModel _mainWindowModel { get; set; }

        public MainWindowViewModel(MainWindowModel mainWindowModel)
        {
            _mainWindowModel = mainWindowModel;

            ImportData = new DelegateCommand(SetTextFromClipboard);
            ExportData = new DelegateCommand(SetTextToClipboard, CanExecuteExportData);
            ClearData = new DelegateCommand(ClearDataIn, CanExecuteClearDataIn);

            DataIn.CollectionChanged += DataInCollectionChanged;
            DataOut.CollectionChanged += DataOutCollectionChanged;
        }

        private void DataInCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            (ClearData as DelegateCommand).RaiseCanExecuteChanged();
            //OnPropertyChanged(new PropertyChangedEventArgs(nameof(Logger)));
        }

        private void DataOutCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
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
            ClipboardHelper.SetClipboardData(_mainWindowModel.GetDataAsListList());
        }

        private bool CanExecuteExportData()
        {
            return DataOut != null && DataOut.Count() != 0;
        }

        private void ClearDataIn()
        {
            _mainWindowModel.DataIn.Clear();
        }

        private bool CanExecuteClearDataIn()
        {
            return DataIn != null && DataIn.Count() != 0;
        }
    }
}
