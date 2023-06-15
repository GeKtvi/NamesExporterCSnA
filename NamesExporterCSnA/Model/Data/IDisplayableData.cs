using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace NamesExporterCSnA.Model.Data
{
    public interface IDisplayableData
    {
        [Display(AutoGenerateField = false)]
        string DataType { get; }

        [Display(Name = "Наименование")]
        string Name { get; set; }

        [Display(Name = "Кол-во")] 
        int Count { get; set; }

        [Display(Name = "Кол-во х2")]
        int CountX2 { get; }

        [Display(Name = "Окр.")]
        int Rounded { get; }

        //[Display(AutoGenerateField = false)]
        //int VendorPalletCount { get; set; }
    }
}