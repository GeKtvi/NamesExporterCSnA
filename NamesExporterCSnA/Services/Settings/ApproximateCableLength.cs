using System.ComponentModel;

namespace NamesExporterCSnA.Services.Settings
{
    public class ApproximateCableLength : INotifyPropertyChanged
    {
        public int BoxWidth { get; set; }
        public int BoxHeight { get; set; }
        public int BoxDepth { get; set; }
        public double K { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public double FinalMultiplier => ((double)BoxWidth / 1000 * (double)BoxHeight / 1000 * (double)BoxDepth / 1000 * K);

        public ApproximateCableLength() { }
    }
}
