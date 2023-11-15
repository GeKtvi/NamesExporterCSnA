using System.ComponentModel;

namespace NamesExporterCSnA.Data.Settings
{
    public class ApproximateCableLength : INotifyPropertyChanged, IApproximateCableLength
    {
        public int BoxWidth { get; set; } = 1000;
        public int BoxHeight { get; set; } = 2000;
        public int BoxDepth { get; set; } = 1000;
        public double K { get; set; } = 1;

        public event PropertyChangedEventHandler PropertyChanged;

        public double FinalMultiplier =>
             Math.Round((BoxWidth / 1000.0 + BoxHeight / 1000.0 + BoxDepth / 1000.0) * K, 2);

        public ApproximateCableLength() { }
    }
}
