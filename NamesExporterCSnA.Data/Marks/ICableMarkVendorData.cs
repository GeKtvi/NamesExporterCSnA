namespace NamesExporterCSnA.Data.Marks
{
    public interface ICableMarkVendorData
    {
        string VendorName { get; }
        public ICableMark[] ExistingMarks { get; }
        public string[] MultiCharacterSymbols { get; set; }
    }
}
