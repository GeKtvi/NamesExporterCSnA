using System;

namespace NamesExporterCSnA.Model.Data.Cables.Exceptions
{
    internal class CableTemplateNotFoundException : Exception
    {
        public CableTemplateNotFoundException(string message) : base(message) { }
    }
}
