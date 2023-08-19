using Prism.Mvvm;

namespace NamesExporterCSnA.Data.UpdateLog
{
    public class UpdateLogger : BindableBase, IUpdateLogger
    {
        public LoggerStatus Status
        {
            get
            {
                lock (FailList)
                {
                    return FailList.Count == 0
                        ? LoggerStatus.NoFails
                        : FailList.AsParallel().Any(x => x.Type == UpdateFailType.Error) ? LoggerStatus.HasErrorFails : LoggerStatus.HasExceptionFails;
                }
            }
        }

        public List<UpdateFail> FailList { get; private set; } = new List<UpdateFail>();

        private bool _frozen = false;

        public UpdateLogger() { }

        public void Log(UpdateFail updateFail)
        {
            FailList.Add(updateFail);
            OnLogChanged();
        }

        public void ClearLog()
        {
            FailList.Clear();
            if (_frozen == false)
                OnLogChanged();
        }

        private void OnLogChanged()
        {
            if (_frozen == false)
            {
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Status)));
                OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(FailList)));
            }
        }

        public void FreezeLogNotify()
        {
            _frozen = true;
        }

        public void UnfreezeLogNotify()
        {
            _frozen = false;
            OnLogChanged();
        }
    }
}
