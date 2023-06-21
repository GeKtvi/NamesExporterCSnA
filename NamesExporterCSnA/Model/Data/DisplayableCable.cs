using Microsoft.VisualBasic;
using NamesExporterCSnA.Model.Data.Cables;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace NamesExporterCSnA.Model.Data
{
    public class DisplayableCable : IDisplayableData, IFromGroup<Cable>
    {
        public string DataType { get; } = "Кабель/Провод";

        public string Name { get; set; } = "{NotSet}";

        public int Count { get; set; } = -1;

        public int CountX2 => Count * 2;

        public int VendorPalletCount { get; set; } = 100;

        public int Rounded => (CountX2 - 1) / VendorPalletCount * VendorPalletCount + VendorPalletCount;

        public string Measure => "м";

        public DisplayableCable()
        {

        }

        public IDisplayableData SetFromGrouping(IGrouping<string, Cable> group)
        {
            Name = group.First().FullName;
            Count = group.Count();

            return this;
        }
    }
}
