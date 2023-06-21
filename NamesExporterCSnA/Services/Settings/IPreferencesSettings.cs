using System.ComponentModel;

namespace NamesExporterCSnA.Services.Settings
{
    public interface IPreferencesSettings : INotifyPropertyChanged
    {
        ApproximateCableLength ApproximateCableLength { get; set; }
        string CableMarkSelectedVendorName { get; set; }
        string[] PossibleCableMarkVendorName { get; set; }
    }
}