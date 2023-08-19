using NamesExporterCSnA.Data.Marks;

namespace NamesExporterCSnA.Data
{
    public class DisplayableCableMark : IDisplayableData, IFromGroup<ICableMark>
    {
        public string DataType { get; } = "Маркировка";

        public string Name { get; set; } = "{NotSet}";

        public int Count { get; set; } = -1;

        public int VendorPalletCount { get; set; } = -1;

        public int Rounded => (Count - 1) / VendorPalletCount * VendorPalletCount + VendorPalletCount;

        public string Measure => "шт.";

        public DisplayableCableMark() { }

        public IDisplayableData SetFromGrouping(IGrouping<string, ICableMark> group)
        {
            Name = group.First().FullName;
            Count = group.Count();
            VendorPalletCount = group.First().PackageAmount;

            return this;
        }
    }
}
