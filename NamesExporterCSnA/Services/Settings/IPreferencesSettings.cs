using System;
using System.ComponentModel;
using System.Xml.Serialization;

namespace NamesExporterCSnA.Services.Settings
{
    public interface IPreferencesSettings : INotifyPropertyChanged
    {
        IApproximateCableLength ApproximateCableLength { get; set; }

        string CableMarkSelectedVendorName { get; set; }
        string[] PossibleCableMarkVendorName { get; set; }

        event Action DataConverterSettingChanged;
    }
}