using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.Marks
{
    public interface ICableMarkVendorData
    {
        string VendorName { get; }
        public ICableMark[] ExistingMarks { get; }
        public string[] MultiCharacterSymbols { get; set; }
    }
}
