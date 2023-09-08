using DynamicData.Binding;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.UpdateLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Windows.Threading;

namespace NamesExporterCSnA.Model
{
    public class MainWindowModel
    {
        public ObservableCollectionExtended<MaxExportedCable> DataIn { get; }
        public ReadOnlyObservableCollection<IDisplayableData> DataOut => new ReadOnlyObservableCollection<IDisplayableData>(_dataOut);
        public IUpdateLogger Logger => _converter.Logger;
        public bool IsUpdateFrozen { get; set; } = false;
        public IObservable<double> SettingsChanged { get; }

        private readonly ObservableCollectionExtended<IDisplayableData> _dataOut;
        private readonly Dispatcher _dispatcher;
        private DataConverter _converter;

        public MainWindowModel(DataConverter converter)
        {
            DataIn = new ObservableCollectionExtended<MaxExportedCable>();
            _dataOut = new ObservableCollectionExtended<IDisplayableData>();

            _converter = converter;
            SettingsChanged = _converter.Settings.WhenAnyPropertyChanged()
                                .Select(x => x.ApproximateCableLength.K);

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

        public void SetTextFromClipboard()
        {
            List<string[]> data = ClipboardHelper.ParseClipboardData();
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

        public void UpdateDataOut()
        {
            #region Debug
#if DEBUG
            Debug.WriteLine($"Start UpdateDataOut - {DateTime.Now}");
            Stopwatch sw = Stopwatch.StartNew();
#endif
            #endregion
            List<IDisplayableData> data = _converter.Convert(DataIn.ToList());
           
            _dispatcher.BeginInvoke(() =>
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
