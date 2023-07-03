using NamesExporterCSnA.Model.Data.Marks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.Cables
{
    class PurchasedCable : ICable
    {
        public string FullName => CableType;

        public string CableType { get; set; }
        public double Length { get; set; }
        public string SchemeName { get; set; }

        public bool HasFixedLength => true;
        public int WireCount => 0;
        public int WirePairs => 0;
        public double WireSection => 0;
    }
}
