using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace NamesExporterCSnA.Model.Data
{
    public class CablesParser
    {
        private string[] _whiteList = new string[] { "ШВВП", "ПУГВ", "КГВВ" };
        public CablesParser()
        {

        }

        public List<Cable> Parse(List<MaxExportedCable> cables)
        {
            cables = CopyCablesValue(cables);
            RemoveCableNumbers(ref cables);

            cables = FiltrateByWhiteList(cables);

            List<Cable> parsedCables = new List<Cable>();

            foreach (var cable in cables)
            {
                
                int spaceInd = cable.WireName.LastIndexOf(' ');
                string cableType =
                        cable.WireName.Substring(0,
                            spaceInd);

                string cableData =
                         cable.WireName.Substring(spaceInd,
                            cable.WireName.Length - spaceInd);

                int x_ind = cableData.LastIndexOf('х');
                string wireCountS = cableData.Substring(0, x_ind);
                int wireCount = Convert.ToInt32(wireCountS);

                string wireSectionS = cableData.Substring(x_ind+1, cableData.Length - x_ind -1);
                double wireSection = Convert.ToDouble(wireSectionS);

                Cable parsedCable = new Cable()
                {
                    SchemeName = cable.SchemeName,
                    CableType = cableType,
                    WireSection = wireSection,
                    WireCount = wireCount
                };
                parsedCables.Add(parsedCable);
            }
            return parsedCables;
        }

        private List<MaxExportedCable> FiltrateByWhiteList(List<MaxExportedCable> cables)
        {
            List<MaxExportedCable> cablesCopy = new List<MaxExportedCable>();
            foreach (var cable in cables)
            {
                foreach (var pattern in _whiteList)
                {
                    Regex regex = new Regex(pattern);
                    if (regex.IsMatch(cable.WireName))
                    {
                        cablesCopy.Add(cable);
                    }

                }
            }
            return cablesCopy;
        }

        private void RemoveCableNumbers(ref List<MaxExportedCable> cables)
        {
            Regex regex = new Regex(@"\(\d+\)");

            foreach (var cable in cables)
            {
                if (cable.SchemeName.Length > 0)
                {
                    MatchCollection matches = regex.Matches(cable.SchemeName);
                    foreach (Match match in matches)
                        if (match.Success)
                            cable.SchemeName = cable.SchemeName.Replace(match.Value, String.Empty);
                }
            }
        }

        private List<MaxExportedCable> CopyCablesValue(List<MaxExportedCable> cables)
        {
            List<MaxExportedCable> cablesCopy = new List<MaxExportedCable>();

            foreach (var cable in cables)
                cablesCopy.Add(new MaxExportedCable(cable));

            return cablesCopy;
        }
    }
}
