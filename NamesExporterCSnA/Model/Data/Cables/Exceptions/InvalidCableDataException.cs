using System;

namespace NamesExporterCSnA.Model.Data.Cables.Exceptions
{
    internal class InvalidCableDataException : Exception
    {
        public InvalidCableDataException(string message) : base(message) { }
    }
}
