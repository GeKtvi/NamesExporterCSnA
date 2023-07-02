namespace NamesExporterCSnA.Model.Data.Cables
{
    public interface ICable
    {
        string CableType { get; set; }
        string FullName { get; }
        bool HasFixedLength { get; set; }
        double Length { get; set; }
        string SchemeName { get; set; }
        int WireCount { get; set; }
        int WirePairs { get; set; }
        double WireSection { get; set; }
    }
}