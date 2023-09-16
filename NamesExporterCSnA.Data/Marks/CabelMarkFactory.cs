using NamesExporterCSnA.Data.Cables;
using NamesExporterCSnA.Data.Marks.Exceptions;
using NamesExporterCSnA.Data.Settings;
using NamesExporterCSnA.Data.UpdateLog;
using System.Collections.ObjectModel;

namespace NamesExporterCSnA.Data.Marks
{
    public class CableMarkFactory
    {
        private string _selectedVendorName = "{NotSet}";
        public string SelectedVendorName
        {
            get => _selectedVendorName;
            set
            {
                IEnumerable<CableMarkVendorData> foundVendorData = _cableMarkVendorsData.Where(x => x.VendorName == value);
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
        private CableMarkingWhiteList _cableForMarkingWhiteList;

        private readonly IUpdateLogger _logger;
        private readonly IPreferencesSettings _settings;

        public CableMarkFactory(IUpdateLogger logger, IPreferencesSettings settings, CableMarkVendorData[] cableMarkVendorsData, CableMarkingWhiteList cableForMarkingWhiteList)
        {
            _logger = logger;
            _settings = settings;
            _cableMarkVendorsData = cableMarkVendorsData;
            _cableForMarkingWhiteList = cableForMarkingWhiteList;

            VendorsNames = new ReadOnlyCollection<string>(_cableMarkVendorsData.Select(x => x.VendorName).ToList());
            SelectedVendorName = VendorsNames.First();

            settings.CableMarkSelectedVendorName = SelectedVendorName;
            settings.PossibleCableMarkVendorName = VendorsNames.ToArray();
            settings.PropertyChanged += (s, e) => SelectedVendorName = settings.CableMarkSelectedVendorName;
        }

        public List<ICableMark> CreateMarksForCable(ICable sourceCable)
        {
            if (IsCableValidForMarking(sourceCable) == false)
                return new List<ICableMark>();

            CheckSelectedVendorData();

            List<string> symbolsInCable = SplitSchemeNameToSymbols(sourceCable.SchemeName);

            List<ICableMark> marks = FindMarksForCable(sourceCable, symbolsInCable);

            return marks;
        }

        public List<ICableMark> CreateMarksForCables(List<Cable> sourceCables)
        {
            List<ICableMark> marks = new();
            foreach (Cable sourceCable in sourceCables)
                marks.AddRange(CreateMarksForCable(sourceCable));
            return marks;
        }

        public List<string> SplitSchemeNameToSymbols(string schemeName)
        {
            List<string> symbolsInCable = new();

            foreach (SymbolsMapper MultiCharacterSymbol in _selectedCableMarkVendorsData.SymbolsMappers)
            {
                if (schemeName.Contains(MultiCharacterSymbol.SymbolIn))
                {
                    symbolsInCable.Add(MultiCharacterSymbol.SymbolOut);
                    schemeName = schemeName.Replace(MultiCharacterSymbol.SymbolIn, string.Empty);
                }
            }

            foreach (char symbol in schemeName)
                symbolsInCable.Add(symbol.ToString());

            return symbolsInCable;
        }

        private List<ICableMark> FindMarksForCable(ICable sourceCable, List<string> symbolsInCable)
        {
            List<ICableMark> marks = new();
            List<string> symbolsExceptedSectionNotFound = new List<string>();

            foreach (string symbol in symbolsInCable)
            {
                IEnumerable<ICableMark> foundMarks = _selectedCableMarkVendorsData.ExistingMarks.Where(x => x.Symbol == symbol);

                if (foundMarks.Count() == 0)
                {
                    LogSymbolNotFound(sourceCable, symbol);
                    continue;
                }

                ICableMark fendedMark = foundMarks
                    .Where(item => item.MaxSection >= sourceCable.WireSection && item.MinSection <= sourceCable.WireSection)
                    .MaxBy(x => x.MaxSection);

                if (fendedMark != null)
                    for (int i = 0; i < sourceCable.WireCount; i++)
                    {
                        marks.Add(fendedMark);
                        marks.Add(fendedMark); //х2 (У кабеля два конца и марки две)
                    }
                else
                    symbolsExceptedSectionNotFound.Add(symbol);
            }

            if (symbolsExceptedSectionNotFound.Any())
                LogMarkExceptSectionNotFound(sourceCable, symbolsExceptedSectionNotFound.Distinct());

            return marks;
        }

        private void CheckSelectedVendorData()
        {
            foreach (CableMarkVendorData item in _cableMarkVendorsData)
                if (item == _selectedCableMarkVendorsData)
                    return;

            throw new InvalidOperationException("Установленный объект отсутствует в списке");
        }

        private bool IsCableValidForMarking(ICable sourceCable)
        {
            if (_cableForMarkingWhiteList.Where(x => sourceCable.CableType.Contains(x)).ToList().Count != 0)
            {
                return true;
            }
            else
            {
                LogCableNotAllowedForMarking(sourceCable);
                return false;
            }
        }

        private void LogCableNotAllowedForMarking(ICable sourceCable)
        {
            _logger.Log(new UpdateFail()
            {
                Message = "Кабель не разрешен для маркировки",
                SchemeName = sourceCable.SchemeName,
                WireName = sourceCable.CableType,
                Source = "Модуль маркировки",
                Type = UpdateFailType.Exception
            });
        }

        private void LogMarkExceptSectionNotFound(ICable sourceCable, IEnumerable<string> symbols)
        {
            _logger.Log(new UpdateFail()
            {
                Message = $"Маркировка для символа(ов): {string.Join(", ", symbols)} \n" +
                $"не найдена, для кабеля с сечением жилы {sourceCable.WireSection}",
                SchemeName = sourceCable.SchemeName,
                WireName = sourceCable.CableType,
                Source = "Модуль маркировки",
                Type = UpdateFailType.Exception
            });
        }

        private void LogSymbolNotFound(ICable sourceCable, string symbol)
        {
            _logger.Log(new UpdateFail()
            {
                Message = $"Символ - {symbol}, не найден в каталоге производителя",
                SchemeName = sourceCable.SchemeName,
                WireName = sourceCable.CableType,
                Source = "Модуль маркировки",
                Type = UpdateFailType.Error
            });
        }
    }
}
