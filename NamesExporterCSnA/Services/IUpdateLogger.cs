using System.Collections.Generic;

namespace NamesExporterCSnA.Services
{
    public interface IUpdateLogger
    {
        LoggerStatus Status { get; }
        List<UpdateFail> FailList { get; }
        void Log(UpdateFail updateFail);
        void ClearLog();
    }
}
