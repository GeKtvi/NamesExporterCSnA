using System;
using System.Collections.Generic;
using System.Linq;
using GeKtviWpfToolkit;
using ModernWpf.Controls;
using NamesExporterCSnA.Model.Data.Marks.Exceptions;

namespace NamesExporterCSnA.Model.Data.Marks
{
    class CableMarkFabric
    {
        public CableMarkVendorData SelectedCableMarkVendorsData { get; set; }
        public CableMarkVendorData[] CableMarkVendorsData { get; private set; }

        public CableMarkFabric()
        {
            CableMarkVendorsData = AppConfigHelper.LoadConfig<CableMarkVendorData[]>("CableMarks.config");
            SelectedCableMarkVendorsData = CableMarkVendorsData.First();
        }

        public List<ICableMark> GetMarksByCableName(Cable sourceCable)
        {
            CheckSelectedItem();

            sourceCable = new Cable(sourceCable);

            List<string> symbolsInCable = SplitSchemeNameToSymbols(sourceCable.SchemeName);

            List<ICableMark> marks = new();

            foreach (var symbol in symbolsInCable)
            {
                IEnumerable<ICableMark> foundMarks = SelectedCableMarkVendorsData.ExistingMarks.Where(x => x.Symbol == symbol);

                if (foundMarks.Count() == 0)
                    throw new SymbolNotFoundException($"Символ \"{symbol}\" не найден в каталоге");

                ICableMark findetMark = foundMarks
                    .Where(item => item.MaxSection >= sourceCable.WireSection && item.MinSection <= sourceCable.WireSection)
                    .MaxBy(x => x.MaxSection);

                if (findetMark != null)
                    marks.Add(findetMark);
            }

            return marks;
        }

        public List<string> SplitSchemeNameToSymbols(string schemeName)
        {
            List<string> symbolsInCable = new();


            foreach (var MultiCharacterSymbol in SelectedCableMarkVendorsData.MultiCharacterSymbols)
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
            foreach (var item in CableMarkVendorsData)
                if (item == SelectedCableMarkVendorsData)
                    isFound = true;

            if (isFound == false)
                throw new InvalidOperationException("Установленный объект отсутствует в списке");
        }
    }
}
