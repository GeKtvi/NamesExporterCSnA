using NamesExporterCSnA.Data.Cables;

namespace NamesExporterCSnA.Data
{
    public class DisplayableCable : IDisplayableData, IFromGroup<ICable>
    {
        public string DataType { get; } = "Кабель/Провод";

        public string Name { get; set; } = "{NotSet}";

        public int Count { get; set; } = -1;

        //public int CountX2 => Count * 2;

        public int VendorPalletCount { get; set; } = 100;

        public int Rounded => (Count - 1) / VendorPalletCount * VendorPalletCount + VendorPalletCount;

        public string Measure { get; private set; }

        public DisplayableCable()
        {

        }

        public IDisplayableData SetFromGrouping(IGrouping<string, ICable> group)
        {
            Name = group.First().FullName;
            if (group.First().HasFixedLength)
            {
                Measure = "шт.";
                Count = group.Count();
            }
            else
            {
                Measure = "м";
                double length = 0;
                foreach (ICable item in group)
                    length += item.Length;
                Count = (int)Math.Ceiling(length);
            }

            return this;
        }
    }
}
