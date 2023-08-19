using NamesExporterCSnA.Data.Settings;
using NamesExporterCSnA.Model;
using Prism.Mvvm;

namespace NamesExporterCSnA.ViewModel
{
    public class SettingsWindowViewModel : BindableBase
    {
        public IApproximateCableLength ApproximateCableLength
        {
            get => _model.ApproximateCableLength;
            set => _model.ApproximateCableLength = value;
        }

        public string CableMarkSelectedVendorName
        {
            get => _model.CableMarkSelectedVendorName;
            set => _model.CableMarkSelectedVendorName = value;
        }

        public string[] PossibleCableMarkVendorName
        {
            get => _model.PossibleCableMarkVendorName;
            set => _model.PossibleCableMarkVendorName = value;
        }

        private readonly SettingsWindowModel _model;

        public SettingsWindowViewModel(SettingsWindowModel model) 
        {
            _model = model;
            _model.PropertyChanged += (s, e) => OnPropertyChanged(e);
        }
    }
}
