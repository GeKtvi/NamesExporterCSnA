using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace NamesExporterCSnA.Services.UpdateLog
{
    public class UpdateLogger : BindableBase, IUpdateLogger
    {
        public LoggerStatus Status
        {
            get
            {
                if (FailList.Count == 0)
                    return LoggerStatus.NoFails;
                if (FailList.Where(x => x.Type == UpdateFailType.Error).Count() != 0)
                    return LoggerStatus.HasErrorFails;
                else
                    return LoggerStatus.HasExceptionFails;
            }
        }

        public List<UpdateFail> FailList { get; private set; } = new List<UpdateFail>();

        public UpdateLogger() { }

        public void Log(UpdateFail updateFail)  
        { 
            FailList.Add(updateFail);
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Status)));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(FailList)));
        }

        public void ClearLog()
        {
            FailList.Clear();
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Status)));
            OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(FailList)));
        }
    }
}
