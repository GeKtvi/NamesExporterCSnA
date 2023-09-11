using DynamicData;
using DynamicData.Binding;
using NamesExporterCSnA.Data.UpdateLog;
using ReactiveUI;
using ReactiveUI.Fody.Helpers;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;

namespace NamesExporterCSnA.ViewModel
{
    public class UpdateLoggerViewModel : ReactiveObject
    {
        [Reactive]
        public ReadOnlyObservableCollection<UpdateFail> FailList { get; }

        [Reactive]
        public LoggerStatus Status { get; private set; }

        public UpdateLoggerViewModel(IUpdateLogger logger)
        {
            logger.FailList.ToObservableChangeSet()
                .ObserveOn(RxApp.MainThreadScheduler)
                .Bind(out ReadOnlyObservableCollection<UpdateFail> failList)
                .Subscribe();
            FailList = failList;

            logger.WhenAnyPropertyChanged()
                .Throttle(TimeSpan.FromMilliseconds(50))
                .ObserveOn(RxApp.MainThreadScheduler)
                .Select(x => x.Status)
                .BindTo(this, x => x.Status);
        }
    }
}
