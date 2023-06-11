using ModernWpf.Controls;
using NamesExporterCSnA.Model.Data.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data
{
    public class Cable
    {
        public string CableType { get; set; } = "{NotSet}";
        public string SchemeName { get; set; } = "{NotSet}";
        public double WireSection { get; set; } = 0;
        public int WireCount { get; set; } = 0;
        public string NormativeDocument { get; set; } = "{NotSet}";

        public Cable() { }

        public Cable(string cableType, string schemeName, double wireSection, int wireCount, string normativeDocument)
        {
            CableType = cableType;
            SchemeName = schemeName;
            WireSection = wireSection;
            WireCount = wireCount;
            NormativeDocument = normativeDocument;
        }

        public Cable(Cable cable)
        {
            CableType = cable.CableType;
            SchemeName = cable.SchemeName;
            WireSection = cable.WireSection;
            WireCount = cable.WireCount;
            NormativeDocument = cable.NormativeDocument;
        }

        public override bool Equals(object obj)
        {
            Cable objectToCompare;
            if (obj is Cable == false)
                return false;
            else
                objectToCompare = (Cable)obj;

            return (objectToCompare.CableType == CableType) &&
                   (objectToCompare.SchemeName == SchemeName) &&
                   (objectToCompare.WireSection == WireSection) &&
                   (objectToCompare.WireCount == WireCount) &&
                   (objectToCompare.NormativeDocument == NormativeDocument);
        }
    }
}
