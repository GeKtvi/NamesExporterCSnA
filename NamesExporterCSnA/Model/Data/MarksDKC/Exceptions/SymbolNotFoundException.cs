using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.MarksDKC.Exceptions
{
    class SymbolNotFoundException : Exception
    {
        public SymbolNotFoundException(string message) : base(message) { }
    }
}
