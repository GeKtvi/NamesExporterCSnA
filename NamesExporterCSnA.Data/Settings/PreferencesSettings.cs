using System.ComponentModel;
using System.Xml.Serialization;

namespace NamesExporterCSnA.Data.Settings
{
    [XmlInclude(typeof(ApproximateCableLength))]
    public class PreferencesSettings : IPreferencesSettings
    {
        private ApproximateCableLength _approximateCableLength = new ApproximateCableLength();

        [XmlIgnore]
        public IApproximateCableLength ApproximateCableLength
        {
            get => _approximateCableLength;
            set
            {
                _approximateCableLength = (ApproximateCableLength)value;
                _approximateCableLength.PropertyChanged += (s, e) =>
                {
                    if (e.PropertyName != nameof(ApproximateCableLength.FinalMultiplier))
                        OnPropertyChanged(new PropertyChangedEventArgs(nameof(ApproximateCableLength)));
                };
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ApproximateCableLength ApproximateCableLengthSerialized
        {
            get => (ApproximateCableLength)ApproximateCableLength;
            set => ApproximateCableLength = value;
        }

        public string CableMarkSelectedVendorName { get; set; } = "{NotSet}";
        public string[] PossibleCableMarkVendorName { get; set; } = { "{NotSet}" };

        public event PropertyChangedEventHandler? PropertyChanged;

        public PreferencesSettings() { }

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
