using DynamicData.Binding;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace NamesExporterCSnA.Data.UpdateLog
{
    public interface IUpdateLogger : INotifyPropertyChanged
    {
        LoggerStatus Status { get; }
        ReadOnlyObservableCollection<UpdateFail> FailList { get; }
        void Log(UpdateFail updateFail);
        void ClearLog();
    }
}
