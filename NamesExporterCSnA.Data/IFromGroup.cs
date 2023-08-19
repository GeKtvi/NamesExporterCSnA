namespace NamesExporterCSnA.Data
{
    public interface IFromGroup<T>
    {
        IDisplayableData SetFromGrouping(IGrouping<string, T> group);
    }
}
