using NamesExporterCSnA.Model.Data.Marks;

namespace NamesExporterCSnA.Model.Data.Cables
{
    public interface ICable :IFullName
    {
        string CableType { get; }
        bool HasFixedLength { get; }
        double Length { get; }
        string SchemeName { get; }
        int WireCount { get; }
        int WirePairs { get; }
        double WireSection { get; }
    }
}