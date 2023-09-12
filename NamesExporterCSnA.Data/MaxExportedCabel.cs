using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace NamesExporterCSnA.Data
{
    public class MaxExportedCable : INotifyPropertyChanged
    {
        [Display(Name = "Обозначение")]
        public string SchemeName { get; set; } = string.Empty;

        [Display(Name = "Наименование")]
        public string WireName { get; set; } = string.Empty;

        public MaxExportedCable() { }

        public MaxExportedCable(MaxExportedCable maxExportedCable)
        {
            SchemeName = maxExportedCable.SchemeName;
            WireName = maxExportedCable.WireName;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
