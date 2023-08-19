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
                        OnDataConverterSettingChanged(new PropertyChangedEventArgs(nameof(ApproximateCableLength)));

                };
            }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public ApproximateCableLength ApproximateCableLengthSerialized
        {
            get => (ApproximateCableLength)ApproximateCableLength;
            set => ApproximateCableLength = value;
        }

        public string CableMarkSelectedVendorName { get; set; }
        public string[] PossibleCableMarkVendorName { get; set; }

        public event Action DataConverterSettingChanged;
        public event PropertyChangedEventHandler PropertyChanged;

        public PreferencesSettings() { }

        protected void OnDataConverterSettingChanged(PropertyChangedEventArgs eventArgs)
        {
            DataConverterSettingChanged?.Invoke();
        }

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
            OnDataConverterSettingChanged(eventArgs);
        }
    }
}
