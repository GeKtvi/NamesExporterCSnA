using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NamesExporterCSnA.Model.Data.Marks
{
    [XmlInclude(typeof(CableMark))]
    [XmlInclude(typeof(SymbolsMapper))]
    public class CableMarkVendorData
    {
        public string VendorName { get; set; } = "{NotSet}";
        public CableMark[] ExistingMarks { get; set; } 
        public SymbolsMapper[] SymbolsMappers { get; set; }
    }
}
