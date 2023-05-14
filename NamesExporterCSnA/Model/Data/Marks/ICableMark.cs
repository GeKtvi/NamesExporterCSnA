using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data.Marks
{
    public interface ICableMark
    {
        public string VendorCode { get; set; }
        public string Symbol { get; set; }
        public double MinSection { get; set; }
        public double MaxSection { get; set; }

        public int PackageAmount { get; set; }

        public string FullName { get; }
    }
}
