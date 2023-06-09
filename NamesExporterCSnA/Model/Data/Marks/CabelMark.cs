using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace NamesExporterCSnA.Model.Data.Marks
{
    public class CableMark : ICableMark
    {
        public string VendorCode { get; set; } = "{NotSet}";
        public string Symbol { get; set; } = "{NotSet}";
        public double MinSection { get; set; } = -1;
        public double MaxSection { get; set; } = -1;
        public int PackageAmount { get; set; } = 200;
        public string Template { get; set; } = "{NotSet}";

        public string FullName 
        { 
            get
            {
                string template = Template;
                foreach (var prop in this.GetType().GetProperties())
                    if(prop.Name != nameof(FullName))
                        template = template.Replace('{' + prop.Name + '}', prop.GetValue(this).ToString());
                return template;
            }
        }
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

        public override bool Equals(object obj)
        {
            if (obj is CableMark == false)
                return false;

            CableMark objectToCompare = obj as CableMark;

            return (objectToCompare.VendorCode == VendorCode) &&
                    (objectToCompare.Symbol == Symbol) &&
                    (objectToCompare.MinSection == MinSection) &&
                    (objectToCompare.MaxSection == MaxSection) &&
                    (objectToCompare.PackageAmount == PackageAmount) &&
                    (objectToCompare.Template == Template) &&
                    (objectToCompare.FullName == FullName);
        }
    }
}
