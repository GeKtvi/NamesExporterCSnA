#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
using NamesExporterCSnA.Data.Cables;

namespace NamesExporterCSnA.Data.Marks
{
    [Equals]
    public class CableMark : FullNameBase, ICableMark, IFullName
    {
        public string VendorCode { get; set; } = "{NotSet}";
        public string Symbol { get; set; } = "{NotSet}";
        public double MinSection { get; set; } = -1;
        public double MaxSection { get; set; } = -1;
        public int PackageAmount { get; set; } = 200;
        public string Template { get; set; } = "{NotSet}";

        public override string FullName => GetFullName(Template, PropertyHolder<CableMark>.GetProperties());

        public CableMark() { }

        public CableMark(string vendorCode, string symbol, double minSection, double maxSection)
        {
            VendorCode = vendorCode;
            Symbol = symbol;
            MinSection = minSection;
            MaxSection = maxSection;
        }

        public CableMark(CableMark markDKC)
        {
            VendorCode = markDKC.VendorCode;
            Symbol = markDKC.Symbol;
            MinSection = markDKC.MinSection;
            MaxSection = markDKC.MaxSection;
        }

        public static bool operator ==(CableMark left, CableMark right) => Operator.Weave(left, right);
        public static bool operator !=(CableMark left, CableMark right) => Operator.Weave(left, right);
    }
}

#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)