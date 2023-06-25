using NamesExporterCSnA.Services.Settings;
using Prism.Mvvm;

namespace NamesExporterCSnA.Model
{
    public class SettingsWindowModel : BindableBase
    {
        public IApproximateCableLength ApproximateCableLength
        {
            get => _preferencesSettings.ApproximateCableLength;
            set => _preferencesSettings.ApproximateCableLength = value;
        }

        public string CableMarkSelectedVendorName
        {
            get => _preferencesSettings.CableMarkSelectedVendorName;
            set => _preferencesSettings.CableMarkSelectedVendorName = value;
        }

        public string[] PossibleCableMarkVendorName
        {
            get => _preferencesSettings.PossibleCableMarkVendorName;
            set => _preferencesSettings.PossibleCableMarkVendorName = value;
        }

        private readonly IPreferencesSettings _preferencesSettings;

        public SettingsWindowModel(IPreferencesSettings settings) 
        {
            _preferencesSettings = settings;
            _preferencesSettings.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }
    }
}
