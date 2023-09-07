using DynamicData;
using DynamicData.Binding;
using Prism.Mvvm;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NamesExporterCSnA.Data.UpdateLog
{
    public class UpdateLogger : BindableBase, INotifyPropertyChanged, IUpdateLogger
    {
        public LoggerStatus Status
        {
            get
            {
                lock (_failList)
                {
                    return _failList.Count == 0
                        ? LoggerStatus.NoFails
                        : _failList.Items.AsParallel().Any(x => x.Type == UpdateFailType.Error) ? LoggerStatus.HasErrorFails : LoggerStatus.HasExceptionFails;
                }
            }
        }

        public ReadOnlyObservableCollection<UpdateFail> FailList { get; }

        private SourceList<UpdateFail> _failList = new SourceList<UpdateFail>();

        public UpdateLogger() 
        {
            _failList.Connect()
                .Bind(out ReadOnlyObservableCollection<UpdateFail> failList);
            FailList = failList;
        }

        public void Log(UpdateFail updateFail)
        {
            _failList.Add(updateFail);
            OnLogChanged();
        }

        public void ClearLog()
        {
            _failList.Clear();
            OnLogChanged();
        }

        private void OnLogChanged()
        {
            OnPropertyChanged(new PropertyChangedEventArgs(nameof(Status)));
        }
    }
}
