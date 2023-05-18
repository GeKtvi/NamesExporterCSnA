using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using GeKtviWpfToolkit;
using NamesExporterCSnA.Model.Data.Marks.Exceptions;

namespace NamesExporterCSnA.Model.Data.Marks
{
    class CableMarkFabric
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
            }
        }

        public ReadOnlyCollection<string> VendorsNames { get; private set; }

        private CableMarkVendorData _selectedCableMarkVendorsData;
        private readonly CableMarkVendorData[] _cableMarkVendorsData;

        public CableMarkFabric()
        {
            _cableMarkVendorsData = AppConfigHelper.LoadConfig<CableMarkVendorData[]>("CableMarks.config");

            VendorsNames = new ReadOnlyCollection<string>(_cableMarkVendorsData.Select(x => x.VendorName).ToList());
            SelectedVendorName = VendorsNames.First();
        }

        public List<ICableMark> GetMarksByCableName(Cable sourceCable)
        {
            CheckSelectedItem();

            sourceCable = new Cable(sourceCable);

            List<string> symbolsInCable = SplitSchemeNameToSymbols(sourceCable.SchemeName);

            List<ICableMark> marks = new();

            foreach (var symbol in symbolsInCable)
            {
                IEnumerable<ICableMark> foundMarks = _selectedCableMarkVendorsData.ExistingMarks.Where(x => x.Symbol == symbol);

                if (foundMarks.Count() == 0)
                    throw new SymbolNotFoundException($"Символ \"{symbol}\" не найден в каталоге");

                ICableMark findetMark = foundMarks
                    .Where(item => item.MaxSection >= sourceCable.WireSection && item.MinSection <= sourceCable.WireSection)
                    .MaxBy(x => x.MaxSection);

                if (findetMark != null)
                    for (int i = 0; i < sourceCable.WireCount; i++)
                        marks.Add(findetMark);
            }

            return marks;
        }

        public List<string> SplitSchemeNameToSymbols(string schemeName)
        {
            List<string> symbolsInCable = new();


            foreach (var MultiCharacterSymbol in _selectedCableMarkVendorsData.MultiCharacterSymbols)
            {
                if (schemeName.Contains(MultiCharacterSymbol))
                {
                    symbolsInCable.Add(MultiCharacterSymbol);
                    schemeName = schemeName.Replace(MultiCharacterSymbol, String.Empty);
                }
            }

            foreach (char symbol in schemeName)
                symbolsInCable.Add(symbol.ToString());

            return symbolsInCable;
        }

        private void CheckSelectedItem()
        {
            bool isFound = false;
            foreach (var item in _cableMarkVendorsData)
                if (item == _selectedCableMarkVendorsData)
                    isFound = true;

            if (isFound == false)
                throw new InvalidOperationException("Установленный объект отсутствует в списке");
        }
    }
}
