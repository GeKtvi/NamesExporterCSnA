using GeKtviWpfToolkit;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NamesExporterCSnA.Data;
using NamesExporterCSnA.Data.Cables;
using NamesExporterCSnA.Data.Marks;
using NamesExporterCSnA.Data.Marks.Exceptions;
using NamesExporterCSnA.Data.Settings;
using NamesExporterCSnA.Data.UpdateLog;

namespace NamesExporterCSnA.Tests
{
    [TestClass]
    public class NamesExporterCSnACommonTests
    {
        [TestMethod]
        public void CreateMarksForCable_SomeCables_ReturnedSomeDKCCableMark()
        {
            List<Cable> cables = new List<Cable>()
            {
                new Cable()
                {
                    CableType = "ШВВП",
                    NormativeDocument = "",
                    SchemeName = "0",
                    WireCount = 1,
                    WireSection = 1.4
                },
                new Cable()
                {
                    CableType = "ПуГВ",
                    NormativeDocument = "",
                    SchemeName = "E",
                    WireCount = 1,
                    WireSection = 1.5
                },
                new Cable()
                {
                    CableType = "ШВВП",
                    NormativeDocument = "",
                    SchemeName = "PE",
                    WireCount = 1,
                    WireSection = 1.4
                }
            };

            List<ICableMark> cableMarksExpected = new List<ICableMark>()
            {
                new CableMark()
                {
                     VendorCode = "MKF0S1",
                     Symbol = "0",
                     MinSection = 0.5,
                     MaxSection = 1.5,
                     PackageAmount = 200,
                     Template = "Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorCode}, ДКС"
                },
                new CableMark()
                {
                     VendorCode = "MKF0S1",
                     Symbol = "0",
                     MinSection = 0.5,
                     MaxSection = 1.5,
                     PackageAmount = 200,
                     Template = "Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorCode}, ДКС"
                },
                new CableMark()
                {
                     VendorCode = "MKCES2",
                     Symbol = "E",
                     MinSection = 1.5,
                     MaxSection = 2.5,
                     PackageAmount = 200,
                     Template = "Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorCode}, ДКС"
                },
                new CableMark()
                {
                     VendorCode = "MKCES2",
                     Symbol = "E",
                     MinSection = 1.5,
                     MaxSection = 2.5,
                     PackageAmount = 200,
                     Template = "Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorCode}, ДКС"
                },
                new CableMark()
                {
                     VendorCode = "MKSGS1EARTH",
                     Symbol = "земля",
                     MinSection = 0.5,
                     MaxSection = 1.5,
                     PackageAmount = 200,
                     Template = "Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorCode}, ДКС"
                },
                new CableMark()
                {
                     VendorCode = "MKSGS1EARTH",
                     Symbol = "земля",
                     MinSection = 0.5,
                     MaxSection = 1.5,
                     PackageAmount = 200,
                     Template = "Ручная маркировка кабеля, сечением {MinSection}-{MaxSection} мм кв., символ '{Symbol}', арт. {VendorCode}, ДКС"
                }
            };

            CableMarkFactory factory = new CableMarkFactory(
                new UpdateLogger(), new PreferencesSettings(),
                AppConfigHelper.LoadConfig<CableMarkVendorData[]>("CableMarks.config"),
                AppConfigHelper.LoadConfig<CableMarkingWhiteList>("CableForMarkingWhiteList.config"));
            List<ICableMark> result = factory.CreateMarksForCables(cables);

            CollectionAssert.AreEqual(result, cableMarksExpected);
        }

        [TestMethod]
        public void Parse_SomeMaxCables_ReturnedSomeCables()
        {
            List<MaxExportedCable> maxExportedCable = new List<MaxExportedCable>()
            {
                new MaxExportedCable() { SchemeName = "шина L2(1)", WireName ="Шина медная L2"},
                new MaxExportedCable() { SchemeName = "15/N",       WireName ="ШВВП 2х0,5"},
                new MaxExportedCable() { SchemeName = "PE(28)",     WireName ="ПуГВнг(А)-LS 1х10"},
                new MaxExportedCable() { SchemeName = "A11-1",      WireName ="ПуГВнг(А)-LS 1х0,5"},
                new MaxExportedCable() { SchemeName = "L334",       WireName ="КГВВнг(А)-LS 3х0,75"}
            };

            ApproximateCableLength length = new ApproximateCableLength();
            length.BoxWidth = 1000;
            length.BoxHeight = 1000;
            length.BoxDepth = 1000;
            length.K = 1;

            List<object> cablesResult = new CablesParser(
                new UpdateLogger(),
                length,
                AppConfigHelper.LoadConfig<CablesParserConfig>("CablesParser.config"))
                    .Parse(maxExportedCable).Cast<object>().ToList();
            List<object> cablesExpected = new List<object>()
            {
                new Cable()
                {
                    CableType = "ШВВПнг(А)-LS",
                    NormativeDocument = "{NotSet}",
                    SchemeName = "15/N",
                    WireCount = 2,
                    WireSection = 0.5,
                    Template = "Провод {CableType} {WireCount}х{WireSection}, ТУ 3520-005-50951092-2005, РЭК",
                    Length = 1
                },
                new Cable()
                {
                    CableType = "ПуГВнг(А)-LS",
                    NormativeDocument = "{NotSet}",
                    SchemeName = "PE",
                    WireCount = 1,
                    WireSection = 10,
                    Template = "Провод {CableType} {WireCount}х{WireSection}, {Color}, ТУ 3520-005-50951092-2005, РЭК",
                    Length = 1,
                    HasColor = true,
                    Color = "желто-зеленый"
                },
                new Cable()
                {
                    CableType = "ПуГВнг(А)-LS",
                    NormativeDocument = "{NotSet}",
                    SchemeName = "A11-1",
                    WireCount = 1,
                    WireSection = 0.5,
                    Template = "Провод {CableType} {WireCount}х{WireSection}, {Color}, ТУ 3520-005-50951092-2005, РЭК",
                    Length = 1,
                    HasColor = true,
                    Color = "белый"
                },
                new Cable()
                {
                    CableType = "КГВВнг(А)-LS",
                    NormativeDocument = "{NotSet}",
                    SchemeName = "L334",
                    WireCount = 3,
                    WireSection = 0.75,
                    Template = "Кабель {CableType} {WireCount}х{WireSection}, ТУ 3520-005-50951092-2005, РЭК",
                    Length = 1
                },
            };

            CollectionAssert.AreEqual(cablesResult, cablesExpected);
        }
    }
}

