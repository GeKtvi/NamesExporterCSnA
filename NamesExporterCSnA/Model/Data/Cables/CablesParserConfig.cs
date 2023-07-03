using System.Linq;
using NamesExporterCSnA.Model.Data.Cables.Exceptions;

namespace NamesExporterCSnA.Model.Data.Cables
{
    public class CablesParserConfig
    {
        public CableTemplate[] Templates;

        public ColorMapper DefaultColorMapper;

        public CableTemplate GetTemplate(string cableType)
        {
            var result = Templates.Where(x => cableType.Contains(x.SubCableType));
            if (result.Count() > 0)
                return result.First();
            else
                throw new CableTemplateNotFoundException("TemplateNot");
        }

        public string GetTemplateColorOrDefault(CableTemplate cableTemplate, string schemeName)
        {
            if(cableTemplate.ColorMapper is null)
                return DefaultColorMapper.GetColorFromSchemeName(schemeName);

            return cableTemplate.ColorMapper.GetColorFromSchemeName(schemeName);
        }
    }
}
