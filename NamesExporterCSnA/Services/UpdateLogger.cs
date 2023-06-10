using System.Collections.Generic;

namespace NamesExporterCSnA.Services
{
    internal class UpdateLogger : IUpdateLogger
    {
        public List<UpdateFail> FailList { get; private set; } = new List<UpdateFail>();

        public UpdateLogger() { }

        public void Log(UpdateFail updateFail)
        {
            FailList.Add(updateFail);

            throw new System.NotImplementedException();
        }
    }
}
