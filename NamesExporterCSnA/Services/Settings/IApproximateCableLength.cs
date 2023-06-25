using System.ComponentModel;

namespace NamesExporterCSnA.Services.Settings
{
    public interface IApproximateCableLength
    {
        int BoxDepth { get; set; }
        int BoxHeight { get; set; }
        int BoxWidth { get; set; }
        double FinalMultiplier { get; }
        double K { get; set; }

        event PropertyChangedEventHandler PropertyChanged;
    }
}