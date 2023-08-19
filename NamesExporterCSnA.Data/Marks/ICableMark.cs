namespace NamesExporterCSnA.Data.Marks
{
    public interface ICableMark : IFullName
    {
        public string VendorCode { get; set; }
        public string Symbol { get; set; }
        public double MinSection { get; set; }
        public double MaxSection { get; set; }

        public int PackageAmount { get; set; }
    }
}
