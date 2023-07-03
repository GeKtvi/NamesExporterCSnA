using GeKtviWpfToolkit;
using NamesExporterCSnA.Model.Data.Cables.Exceptions;
using NamesExporterCSnA.Services.Settings;
using NamesExporterCSnA.Services.UpdateLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace NamesExporterCSnA.Model.Data.Cables
{
    public class CablesParser
    {
        public IUpdateLogger Logger { get; private set; }

        private IApproximateCableLength _approximateLength;

        private CablesParserConfig _config;

        public CablesParser(IUpdateLogger logger, IApproximateCableLength approximateLength)
        {
            Logger = logger;

            _config = AppConfigHelper.LoadConfig<CablesParserConfig>("CablesParser.config");
            _approximateLength = approximateLength;
        }

        public List<ICable> Parse(List<MaxExportedCable> cables)
        {
            cables = CopyCablesValue(cables);
            RemoveCableNumbers(ref cables);

            cables = FiltrateByWhiteList(cables);

            List<ICable> parsedCables = new List<ICable>();

            foreach (MaxExportedCable cable in cables)
            {
                CableTemplate template = _config.GetTemplate(cable.WireName);

                try
                {
                    template = _config.GetTemplate(cable.WireName);
                }
                catch (Exception)
                {
                    string cableType = GetCableType(cable); //ШВВП_
                    template = _config.GetTemplate(cableType);
                }

                ICable resultCable = null;

                if(template.ParseOutType == nameof(Cable))
                    resultCable = CreateCable(cable, template);

                if (template.ParseOutType == nameof(PurchasedCable))
                    resultCable = CreatePurchasedCable(cable, template);

                if (resultCable != null)
                    parsedCables.Add(resultCable);
            }
            return parsedCables;
        }

        private Cable CreateCable(MaxExportedCable cable, CableTemplate template)
        {
            double length = template.HasFixedLength ? template.Length : 1 * _approximateLength.FinalMultiplier;
            try
            {
                GetCableData(cable, out int pairCount, out int wireCount, out double wireSection);
                Cable parsedCable = new Cable()
                {
                    SchemeName = cable.SchemeName,
                    CableType = template.FullCableType,
                    WireSection = wireSection,
                    WireCount = wireCount,
                    WirePairs = pairCount,
                    Length = length,
                    HasFixedLength = template.HasFixedLength,
                    HasColor = template.HasColor,
                    Template = template.Template
                };
                if (template.HasColor)
                    parsedCable.Color = _config.GetTemplateColorOrDefault(template, cable.SchemeName);
                return parsedCable;
            }
            catch (InvalidCableDataException)
            {
                LogError("Информация о кабеле должна иметь верный формат \" DхD,D\"", cable);
            }
            return null;
        }

        private PurchasedCable CreatePurchasedCable(MaxExportedCable cable, CableTemplate template)
        {
            return new PurchasedCable()
            {
                SchemeName= cable.SchemeName,
                CableType = template.FullCableType,
            };
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
            if (isCableHasPairs)
                pairCount = Convert.ToInt32(match.Groups["pairCount"].Value);
            wireCount = Convert.ToInt32(match.Groups["wireCount"].Value);
            wireSection = Convert.ToDouble(match.Groups["wireSection"].Value);

            return;
        }

        private List<MaxExportedCable> FiltrateByWhiteList(List<MaxExportedCable> cables)
        {
            List<MaxExportedCable> cablesCopy = new List<MaxExportedCable>();
            foreach (MaxExportedCable cable in cables)
            {
                bool isCableAdded = false;
                foreach (string pattern in _config.Templates.Select(x => x.SubCableType))
                {
                    Regex regex = new Regex('^' +pattern);
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

            foreach (MaxExportedCable cable in cables)
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

            foreach (MaxExportedCable cable in cables)
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
