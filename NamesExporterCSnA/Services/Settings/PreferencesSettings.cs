using System;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Xml.Serialization;

namespace NamesExporterCSnA.Services.Settings
{
    [XmlInclude(typeof(ApproximateCableLength))]
    public class PreferencesSettings : IPreferencesSettings
    {
        private ApproximateCableLength _approximateCableLength = new ApproximateCableLength();
        public ApproximateCableLength ApproximateCableLength
        {
            get => _approximateCableLength;
            set
            {
                _approximateCableLength = value;
                _approximateCableLength.PropertyChanged += (s, e) => OnPropertyChanged(new PropertyChangedEventArgs(nameof(ApproximateCableLength)));
            }
        }
        public string CableMarkSelectedVendorName { get; set; }
        public string[] PossibleCableMarkVendorName { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public PreferencesSettings() { }

        protected void OnPropertyChanged(PropertyChangedEventArgs eventArgs)
        {
            PropertyChanged?.Invoke(this, eventArgs);
        }
    }
}
