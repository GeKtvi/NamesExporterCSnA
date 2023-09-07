using System.ComponentModel;

namespace NamesExporterCSnA.Data.Settings
{
    public interface IPreferencesSettings : INotifyPropertyChanged
    {
        IApproximateCableLength ApproximateCableLength { get; set; }

        string CableMarkSelectedVendorName { get; set; }
        string[] PossibleCableMarkVendorName { get; set; }
    }
}