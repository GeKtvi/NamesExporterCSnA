using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.MarksDKC
{
    public class CableMarkDKC
    {
        public string VendorСode { get; set; } = "{NotSet}";
        public string Symbol { get; set; } = "{NotSet}";
        public double MinSection { get; set; } = -1;
        public double MaxSection { get; set; } = -1;

        public int PackageAmount { get; set; } = 200;

        public string FullName => $"Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorСode}, ДКС";

        public CableMarkDKC() { }

        public CableMarkDKC(string vendorСode, string symbol, double minSection, double maxSection)
        {
            VendorСode = vendorСode;
            Symbol = symbol;
            MinSection = minSection;
            MaxSection = maxSection;
        }

        public CableMarkDKC(CableMarkDKC markDKC) 
        {
            VendorСode = markDKC.VendorСode;
            Symbol = markDKC.Symbol;
            MinSection = markDKC.MinSection;
            MaxSection = markDKC.MaxSection;
        }
    }   
}
