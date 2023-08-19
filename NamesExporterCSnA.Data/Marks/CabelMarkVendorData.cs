using System.Xml.Serialization;

namespace NamesExporterCSnA.Data.Marks
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
