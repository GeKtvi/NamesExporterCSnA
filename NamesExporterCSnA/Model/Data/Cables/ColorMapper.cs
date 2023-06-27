namespace NamesExporterCSnA.Model.Data.Cables
{
    public class ColorMapper
    {
        public ColorMap[] ColorMaps { get; set; }
        public string DefaultColor { get; set; } = "{NotSet}";

        public string GetColorFromSchemeName(string schemeName)
        {
            foreach (var colorMap in ColorMaps)
            {
                if (schemeName.Contains(colorMap.SymbolsForColor))
                    return colorMap.Color;
            }
            return DefaultColor;
        }
    }
}