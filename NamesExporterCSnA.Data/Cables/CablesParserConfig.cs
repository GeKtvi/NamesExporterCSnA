using NamesExporterCSnA.Data.Cables.Exceptions;

namespace NamesExporterCSnA.Data.Cables
{
    public class CablesParserConfig
    {
        public CableTemplate[] Templates;

        public ColorMapper DefaultColorMapper;

        public CableTemplate GetTemplate(string cableType)
        {
            IEnumerable<CableTemplate> result = Templates.Where(x => cableType.Contains(x.SubCableType));
            return result.Count() > 0 ? result.First() : throw new CableTemplateNotFoundException("TemplateNot");
        }

        public string GetTemplateColorOrDefault(CableTemplate cableTemplate, string schemeName)
        {
            return cableTemplate.ColorMapper is null
                ? DefaultColorMapper.GetColorFromSchemeName(schemeName)
                : cableTemplate.ColorMapper.GetColorFromSchemeName(schemeName);
        }
    }
}
