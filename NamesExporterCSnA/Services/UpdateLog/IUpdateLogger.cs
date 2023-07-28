﻿using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace NamesExporterCSnA.Services.UpdateLog
{
    public interface IUpdateLogger
    {
        LoggerStatus Status { get; }
        List<UpdateFail> FailList { get; }
        void Log(UpdateFail updateFail);
        void ClearLog();
        void UnfreezeLogNotify();
        void FreezeLogNotify();
    }
}
