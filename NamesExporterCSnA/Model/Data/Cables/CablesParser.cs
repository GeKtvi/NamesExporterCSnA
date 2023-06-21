using GeKtviWpfToolkit;
using NamesExporterCSnA.Services.UpdateLog;
using NamesExporterCSnA.Model.Data.Cables.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace NamesExporterCSnA.Model.Data.Cables
{
    public class CablesParser
    {
        public IUpdateLogger Logger { get; private set; }

        private CableTemplate[] _templateList;

        public CablesParser(IUpdateLogger logger)
        {
            Logger = logger;
            _templateList = AppConfigHelper.LoadConfig<CableTemplate[]>("CablesParser.config");
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

                try
                {
                    GetCableData(cable, out int pairCount, out int wireCount , out double wireSection);
                    Cable parsedCable = new Cable()
                    {
                        SchemeName = cable.SchemeName,
                        CableType = cableType,
                        WireSection = wireSection,
                        WireCount = wireCount,
                        WirePairs = pairCount,
                        Template = _templateList.Where(x => cableType.Contains(x.SubCableType)).First().Template
                    };
                    parsedCables.Add(parsedCable);
                }
                catch (InvalidCableDataException)
                {
                    LogError("Информация о кабеле должна иметь верный формат \" DхD,D\"", cable);
                }
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

        private static void GetCableData(MaxExportedCable cable, out int pairCount, out int wireCount, out double wireSection)
        {
            pairCount = 0;
            wireCount = 0;
            wireSection = 0;

            bool isCableHasPairs = false;

            Regex signPartRegex = new Regex(@"\s(?<wireCount>\d)+(x|х)(?<wireSection>\d+\,\d+|\d+\.\d+|\d+$)"); //_1х0,5

            if (!signPartRegex.IsMatch(cable.WireName))
            {
                signPartRegex = new Regex(@"\s(?<pairCount>\d)+(x|х)(?<wireCount>\d)+(x|х)(?<wireSection>\d+\,\d+|\d+\.\d+|\d+$)"); //_1х1х0,5
                isCableHasPairs = true;
            }

            if (!signPartRegex.IsMatch(cable.WireName))
                throw new InvalidCableDataException($"Некорректная информация в данных о кабеле: {cable.WireName}");

            Match match = signPartRegex.Match(cable.WireName);
            if(isCableHasPairs)
                pairCount = Convert.ToInt32(match.Groups["pairCount"].Value);
            wireCount = Convert.ToInt32(match.Groups["wireCount"].Value);
            wireSection = Convert.ToDouble(match.Groups["wireSection"].Value);

            return;
        }

        private List<MaxExportedCable> FiltrateByWhiteList(List<MaxExportedCable> cables)
        {
            List<MaxExportedCable> cablesCopy = new List<MaxExportedCable>();
            foreach (var cable in cables)
            {
                bool isCableAdded = false;
                foreach (var pattern in _templateList.Select(x => x.SubCableType))
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
