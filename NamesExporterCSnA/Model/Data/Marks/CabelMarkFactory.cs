using GeKtviWpfToolkit;
using NamesExporterCSnA.Model.Data.Cables;
using NamesExporterCSnA.Model.Data.Marks.Exceptions;
using NamesExporterCSnA.Services.UpdateLog;
using NamesExporterCSnA.Services.Settings;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using NamesExporterCSnA.Properties;

namespace NamesExporterCSnA.Model.Data.Marks
{
    public class CableMarkFactory
    {
        private string _selectedVendorName;
        public string SelectedVendorName
        {
            get => _selectedVendorName;
            set
            {
                var foundVendorData = _cableMarkVendorsData.Where(x => x.VendorName == value);
                if (foundVendorData.Count() < 1)
                    throw new VendorsDataNotFoundException($"В коллекции данных от производителей не найден аргумент: {value}");
                else if (foundVendorData.Count() < 1)
                    throw new VendorsMultiplyDataFoundException($"В коллекции данных от производителей найдены множественные совпадения для аргумента: {value}");

                _selectedCableMarkVendorsData = foundVendorData.First();
                _selectedVendorName = value;

                _settings.CableMarkSelectedVendorName = value;
            }
        }

        public ReadOnlyCollection<string> VendorsNames { get; private set; }

        private CableMarkVendorData _selectedCableMarkVendorsData;
        private readonly CableMarkVendorData[] _cableMarkVendorsData;
        private string[] _cableForMarkingWhiteList;

        private readonly IUpdateLogger _logger;
        private readonly IPreferencesSettings _settings;

        public CableMarkFactory(IUpdateLogger logger, IPreferencesSettings settings)
        {
            _logger = logger;
            _settings = settings;
            _cableMarkVendorsData = AppConfigHelper.LoadConfig<CableMarkVendorData[]>("CableMarks.config");
            _cableForMarkingWhiteList = AppConfigHelper.LoadConfig<string[]>("CableForMarkingWhiteList.config");

            VendorsNames = new ReadOnlyCollection<string>(_cableMarkVendorsData.Select(x => x.VendorName).ToList());
            SelectedVendorName = VendorsNames.First();

            settings.CableMarkSelectedVendorName = SelectedVendorName;
            settings.PossibleCableMarkVendorName = VendorsNames.ToArray();
            settings.PropertyChanged += (s, e) => SelectedVendorName = settings.CableMarkSelectedVendorName;
        }

        public List<ICableMark> CreateMarksForCable(Cable sourceCable)
        {
            if (IsCableValidForMarking(sourceCable) == false)
                return new List<ICableMark>();

            CheckSelectedVendorData();

            sourceCable = new Cable(sourceCable);

            List<string> symbolsInCable = SplitSchemeNameToSymbols(sourceCable.SchemeName);

            List<ICableMark> marks = new();

            foreach (var symbol in symbolsInCable)
            {
                IEnumerable<ICableMark> foundMarks = _selectedCableMarkVendorsData.ExistingMarks.Where(x => x.Symbol == symbol);

                if (foundMarks.Count() == 0)
                    throw new SymbolNotFoundException($"Символ \"{symbol}\" не найден в каталоге"); //TODO перенести в юзер лог

                ICableMark fendedMark = foundMarks
                    .Where(item => item.MaxSection >= sourceCable.WireSection && item.MinSection <= sourceCable.WireSection)
                    .MaxBy(x => x.MaxSection);

                if (fendedMark != null)
                    for (int i = 0; i < sourceCable.WireCount; i++)
                    {
                        marks.Add(fendedMark);
                        marks.Add(fendedMark); //х2 (У кабеля два конца и марки две)
                    }
            }

            return marks;
        }

        public List<ICableMark> CreateMarksForCables(List<Cable> sourceCables)
        {
            List<ICableMark> marks = new();
            foreach (var sourceCable in sourceCables)
                marks.AddRange(CreateMarksForCable(sourceCable));
            return marks;
        }

        public List<string> SplitSchemeNameToSymbols(string schemeName)
        {
            List<string> symbolsInCable = new();

            foreach (var MultiCharacterSymbol in _selectedCableMarkVendorsData.SymbolsMappers)
            {
                if (schemeName.Contains(MultiCharacterSymbol.SymbolIn))
                {
                    symbolsInCable.Add(MultiCharacterSymbol.SymbolOut);
                    schemeName = schemeName.Replace(MultiCharacterSymbol.SymbolIn, String.Empty);
                }
            }

            foreach (char symbol in schemeName)
                symbolsInCable.Add(symbol.ToString());

            return symbolsInCable;
        }

        private void CheckSelectedVendorData()
        {
            bool isFound = false;
            foreach (var item in _cableMarkVendorsData)
                if (item == _selectedCableMarkVendorsData)
                    isFound = true;

            if (isFound == false)
                throw new InvalidOperationException("Установленный объект отсутствует в списке");
        }

        private bool IsCableValidForMarking(Cable sourceCable)
        {
            if (_cableForMarkingWhiteList.Where(x => sourceCable.CableType.Contains(x)).ToList().Count != 0)
            {
                return true;
            }
            else
            {
                _logger.Log(new UpdateFail()
                {
                    Message = "Кабель не разрешен для маркировки",
                    SchemeName = sourceCable.SchemeName,
                    WireName = sourceCable.CableType,
                    Source = "Модуль маркировки",
                    Type = UpdateFailType.Exception
                });
                return false;
            }
        }
    }
}
