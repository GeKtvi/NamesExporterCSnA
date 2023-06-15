using NamesExporterCSnA.Model.Data.Cables;
using NamesExporterCSnA.Model.Data.Marks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data
{
    public class DisplayableCableMark : IDisplayableData, IFromGroup<ICableMark>
    {
        [Display(AutoGenerateField = false)]
        public string DataType { get; } = "Маркировка";

        [Display(Name = "Наименование")]
        public string Name { get; set; } = "{NotSet}";

        [Display(Name = "Кол-во")]
        public int Count { get; set; } = -1;

        [Display(Name = "Кол-во х2")]
        public int CountX2 => Count * 2;

        [Display(AutoGenerateField = false)]
        public int VendorPalletCount { get; set; } = -1;

        [Display(Name = "Окр.")]
        public int Rounded => (CountX2 - 1) / VendorPalletCount * VendorPalletCount + VendorPalletCount;

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
