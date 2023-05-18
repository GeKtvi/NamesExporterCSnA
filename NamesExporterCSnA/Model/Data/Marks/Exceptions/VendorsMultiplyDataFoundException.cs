using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.Marks.Exceptions
{
    class VendorsMultiplyDataFoundException : Exception
    {
        public VendorsMultiplyDataFoundException(string message) : base(message) { }
    }
}
