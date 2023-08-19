using System.ComponentModel.DataAnnotations;

namespace NamesExporterCSnA.Data
{
    public interface IDisplayableData
    {
        [Display(AutoGenerateField = false)]
        string DataType { get; }

        [Display(Name = "Наименование")]
        string Name { get; }

        [Display(Name = "Кол.")]
        int Count { get; }

        [Display(Name = "Окр.")]
        int Rounded { get; }

        [Display(Name = "Е.И.")]
        string Measure { get; }
    }
}