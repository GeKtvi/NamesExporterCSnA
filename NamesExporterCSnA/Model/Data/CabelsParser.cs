using GeKtviWpfToolkit;
using NamesExporterCSnA.Services;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace NamesExporterCSnA.Model.Data
{
    public class CablesParser
    {
        public IUpdateLogger Logger { get; private set; }

        private string[] _whiteList;
        public CablesParser(IUpdateLogger logger)
        {
            Logger = logger;
            _whiteList = AppConfigHelper.LoadConfig<string[]>("CablesParser.config");
        }

        public List<Cable> Parse(List<MaxExportedCable> cables)
        {
            cables = CopyCablesValue(cables);
            RemoveCableNumbers(ref cables);

            cables = FiltrateByWhiteList(cables);

            List<Cable> parsedCables = new List<Cable>();

            foreach (var cable in cables)
            {
                string cableType = GetCableType(cable); //ШВВП_

                string cableData = GetCableData(cable); //_1х0,5
                if (cableData == string.Empty)
                {
                    LogError("Информация о кабеле должна иметь верный формат \" DхD,D\"", cable);
                    continue;
                }

                int wireCount = GetWireCount(cableData); //1х

                double wireSection = GetWireSection(cableData); //х0,5

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

        private static string GetCableType(MaxExportedCable cable)
        {
            Regex cableTypeRegex = new Regex(@".+\s"); //ШВВП_
            string cableType = cableTypeRegex.Match(cable.WireName).Value;
            cableType = cableType.Remove(cableType.Length - 1, 1);  //ШВВП
            return cableType;
        }

        private static string GetCableData(MaxExportedCable cable)
        {
            Regex signPartRegex = new Regex(@"\s\d+(x|х)(\d+\,\d+|\d+\.\d+|\d+$)"); //_1х0,5
            if (!signPartRegex.IsMatch(cable.WireName))
            {
                return string.Empty;
            }
            string cableData = signPartRegex.Match(cable.WireName).Value.Remove(0, 1); //1х0,5
            return cableData;
        }

        private static int GetWireCount(string cableData)
        {
            Regex wireCountRegex = new Regex(@"[\d]+[xх]"); //1х
            string wireCountS = wireCountRegex.Match(cableData).Value; //1
            wireCountS = wireCountS.Remove(wireCountS.Length - 1, 1);
            int wireCount = Convert.ToInt32(wireCountS);
            return wireCount;
        }

        private static double GetWireSection(string cableData)
        {
            Regex wireSectionRegex = new Regex(@"[xх][\d,.]+"); //х0,5
            string wireSectionS = wireSectionRegex.Match(cableData).Value.Remove(0, 1).Replace('.', ','); //0,5
            double wireSection = Convert.ToDouble(wireSectionS);
            return wireSection;
        }

        private List<MaxExportedCable> FiltrateByWhiteList(List<MaxExportedCable> cables)
        {
            List<MaxExportedCable> cablesCopy = new List<MaxExportedCable>();
            foreach (var cable in cables)
            {
                bool isCableAdded = false;
                foreach (var pattern in _whiteList)
                {
                    Regex regex = new Regex(pattern);
                    if (regex.IsMatch(cable.WireName))
                    {
                        cablesCopy.Add(cable);
                        isCableAdded = true;
                    }
                }
                if (!isCableAdded)
                    LogException($"Кабель не прошёл фильтрацию", cable);
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

        private void LogException(string message, MaxExportedCable cable)
        {
            Log(message, UpdateFailType.Exception, cable);
        }

        private void LogError(string message, MaxExportedCable cable)
        {
            Log(message, UpdateFailType.Error, cable);
        }

        private void Log(string message, UpdateFailType updateFailType, MaxExportedCable cable)
        {
            Logger.Log(
                new UpdateFail()
                {
                    Message = message,
                    Type = updateFailType,
                    SchemeName = cable.SchemeName,
                    WireName = cable.WireName,
                    Source = "Преобразование кабеля"
                }
            );
        }
    }
}
