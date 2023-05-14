﻿using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
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

        private string _fullName = "{NotSet}";
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

        public CableMark(string vendorСode, string symbol, double minSection, double maxSection)
        {
            VendorCode = vendorСode;
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
    }
}
