using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace NamesExporterCSnA.Model.Data
{
    public class MaxExportedCable : BindableBase
    {
        [Display(Name = "Обозначение")]
        public string SchemeName { get; set; } = String.Empty;

        [Display(Name = "Наименование")]
        public string WireName { get; set; } = String.Empty;

        public MaxExportedCable() { }

        public MaxExportedCable(MaxExportedCable maxExportedCable) 
        {
            SchemeName = maxExportedCable.SchemeName;
            WireName = maxExportedCable.WireName;
        }

    }
}
