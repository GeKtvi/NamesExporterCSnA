using System.Linq;

namespace NamesExporterCSnA.Model.Data.Cables
{
    public class CablesParserConfig
    {
        public CableTemplate[] Templates;

        public ColorMapper DefaultColorMapper;

        public CableTemplate GetTemplate(string cableType)
        {
            return Templates.Where(x => cableType.Contains(x.SubCableType)).First();
        }

        public string GetTemplateColorOrDefault(CableTemplate cableTemplate, string schemeName)
        {
            if(cableTemplate.ColorMapper is null)
                return DefaultColorMapper.GetColorFromSchemeName(schemeName);

            return cableTemplate.ColorMapper.GetColorFromSchemeName(schemeName);
        }
    }
}
