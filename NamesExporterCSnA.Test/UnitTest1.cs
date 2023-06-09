using NamesExporterCSnA.Model.Data;
using NamesExporterCSnA.Model.Data.Marks;

namespace NamesExporterCSnATests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CreateMarksForCable_SomeCables_ReturnedSomeDKCCableMark()
        {
            List<Cable> cables = new List<Cable>()
            {
                new Cable()
                {
                    CableType = "����",
                    NormativeDocument = "",
                    SchemeName = "0",
                    WireCount = 1,
                    WireSection = 1.4
                },
                new Cable()
                {
                    CableType = "����",
                    NormativeDocument = "",
                    SchemeName = "E",
                    WireCount = 1,
                    WireSection = 1.5
                },
                new Cable()
                {
                    CableType = "����",
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
                     Template = "������ ���������� ������, �������� {MinSection}-{MaxSection} �� ��., ������ '{Symbol}', ���. {VendorCode}, ���"
                },
                new CableMark()
                {
                     VendorCode = "MKCES2",
                     Symbol = "E",
                     MinSection = 1.5,
                     MaxSection = 2.5,
                     PackageAmount = 200,
                     Template = "������ ���������� ������, �������� {MinSection}-{MaxSection} �� ��., ������ '{Symbol}', ���. {VendorCode}, ���"
                },
                new CableMark()
                {
                     VendorCode = "MKSGS1EARTH",
                     Symbol = "�����",
                     MinSection = 0.5,
                     MaxSection = 1.5,
                     PackageAmount = 200,
                     Template = "������ ���������� ������, �������� {MinSection}-{MaxSection} �� ��., ������ '{Symbol}', ���. {VendorCode}, ���"
                }

            };

            CableMarkFactory factory = new CableMarkFactory();
            List<ICableMark> result = factory.CreateMarksForCables(cables);


            CollectionAssert.AreEqual(result, cableMarksExpected);
        }
    }
}

