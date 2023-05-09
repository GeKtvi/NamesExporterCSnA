using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using NamesExporterCSnA.Model.Data.MarksDKC.Exceptions;

namespace NamesExporterCSnA.Model.Data.MarksDKC
{
    class CableMarkDKCFabric
    {
        private readonly CableMarkDKC[] _existingMarks = new CableMarkDKC[]
        {
#region 0,5–1,5 мм2
            new CableMarkDKC(){Symbol = "0",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF0S1"},
            new CableMarkDKC(){Symbol = "1",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF1S1"},
            new CableMarkDKC(){Symbol = "2",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF2S1"},
            new CableMarkDKC(){Symbol = "3",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF3S1"},
            new CableMarkDKC(){Symbol = "4",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF4S1"},
            new CableMarkDKC(){Symbol = "5",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF5S1"},
            new CableMarkDKC(){Symbol = "6",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF6S1"},
            new CableMarkDKC(){Symbol = "7",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF7S1"},
            new CableMarkDKC(){Symbol = "8",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF8S1"},
            new CableMarkDKC(){Symbol = "9",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKF9S1"},
            new CableMarkDKC(){Symbol = "A",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCAS1"},
            new CableMarkDKC(){Symbol = "B",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCBS1"},
            new CableMarkDKC(){Symbol = "C",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCCS1"},
            new CableMarkDKC(){Symbol = "D",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCDS1"},
            new CableMarkDKC(){Symbol = "E",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCES1"},
            new CableMarkDKC(){Symbol = "F",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCFS1"},
            new CableMarkDKC(){Symbol = "G",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCGS1"},
            new CableMarkDKC(){Symbol = "H",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCHS1"},
            new CableMarkDKC(){Symbol = "I",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCIS1"},
            new CableMarkDKC(){Symbol = "J",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCJS1"},
            new CableMarkDKC(){Symbol = "K",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCKS1"},
            new CableMarkDKC(){Symbol = "L",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCLS1"},
            new CableMarkDKC(){Symbol = "M",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCMS1"},
            new CableMarkDKC(){Symbol = "N",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCNS1"},
            new CableMarkDKC(){Symbol = "O",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCOS1"},
            new CableMarkDKC(){Symbol = "P",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCPS1"},
            new CableMarkDKC(){Symbol = "Q",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCQS1"},
            new CableMarkDKC(){Symbol = "R",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCRS1"},
            new CableMarkDKC(){Symbol = "S",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCSS1"},
            new CableMarkDKC(){Symbol = "T",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCTS1"},
            new CableMarkDKC(){Symbol = "U",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCUS1"},
            new CableMarkDKC(){Symbol = "V",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCVS1"},
            new CableMarkDKC(){Symbol = "W",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCWS1"},
            new CableMarkDKC(){Symbol = "X",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCXS1"},
            new CableMarkDKC(){Symbol = "Y",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCYS1"},
            new CableMarkDKC(){Symbol = "Z",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKCZS1"},
            new CableMarkDKC(){Symbol = "Mp",       MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKMPS1"},
            new CableMarkDKC(){Symbol = "+",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSPS1"},
            new CableMarkDKC(){Symbol = "-",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSMS1"},
            new CableMarkDKC(){Symbol = "*",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSGS1"},
            new CableMarkDKC(){Symbol = "(",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSAS1"},
            new CableMarkDKC(){Symbol = ".",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSFS1"},
            new CableMarkDKC(){Symbol = "/",        MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSBS1"},
            new CableMarkDKC(){Symbol = "Ground",   MinSection = 0.5,    MaxSection = 1.5,    VendorСode = "MKSGS1EARTH"},
#endregion
#region 1,5–2,5 мм2
            new CableMarkDKC(){Symbol = "0",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF0S2"},
            new CableMarkDKC(){Symbol = "1",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF1S2"},
            new CableMarkDKC(){Symbol = "2",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF2S2"},
            new CableMarkDKC(){Symbol = "3",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF3S2"},
            new CableMarkDKC(){Symbol = "4",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF4S2"},
            new CableMarkDKC(){Symbol = "5",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF5S2"},
            new CableMarkDKC(){Symbol = "6",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF6S2"},
            new CableMarkDKC(){Symbol = "7",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF7S2"},
            new CableMarkDKC(){Symbol = "8",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF8S2"},
            new CableMarkDKC(){Symbol = "9",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKF9S2"},
            new CableMarkDKC(){Symbol = "A",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCAS2"},
            new CableMarkDKC(){Symbol = "B",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCBS2"},
            new CableMarkDKC(){Symbol = "C",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCCS2"},
            new CableMarkDKC(){Symbol = "D",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCDS2"},
            new CableMarkDKC(){Symbol = "E",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCES2"},
            new CableMarkDKC(){Symbol = "F",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCFS2"},
            new CableMarkDKC(){Symbol = "G",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCGS2"},
            new CableMarkDKC(){Symbol = "H",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCHS2"},
            new CableMarkDKC(){Symbol = "I",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCIS2"},
            new CableMarkDKC(){Symbol = "J",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCJS2"},
            new CableMarkDKC(){Symbol = "K",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCKS2"},
            new CableMarkDKC(){Symbol = "L",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCLS2"},
            new CableMarkDKC(){Symbol = "M",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCMS2"},
            new CableMarkDKC(){Symbol = "N",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCNS2"},
            new CableMarkDKC(){Symbol = "O",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCOS2"},
            new CableMarkDKC(){Symbol = "P",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCPS2"},
            new CableMarkDKC(){Symbol = "Q",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCQS2"},
            new CableMarkDKC(){Symbol = "R",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCRS2"},
            new CableMarkDKC(){Symbol = "S",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCSS2"},
            new CableMarkDKC(){Symbol = "T",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCTS2"},
            new CableMarkDKC(){Symbol = "U",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCUS2"},
            new CableMarkDKC(){Symbol = "V",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCVS2"},
            new CableMarkDKC(){Symbol = "W",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCWS2"},
            new CableMarkDKC(){Symbol = "X",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCXS2"},
            new CableMarkDKC(){Symbol = "Y",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCYS2"},
            new CableMarkDKC(){Symbol = "Z",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKCZS2"},
            new CableMarkDKC(){Symbol = "Mp",       MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKMPS2"},
            new CableMarkDKC(){Symbol = "+",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSPS2"},
            new CableMarkDKC(){Symbol = "-",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSMS2"},
            new CableMarkDKC(){Symbol = "*",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSGS2"},
            new CableMarkDKC(){Symbol = "(",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSAS2"},
            new CableMarkDKC(){Symbol = ".",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSFS2"},
            new CableMarkDKC(){Symbol = "/",        MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSBS2"},
            new CableMarkDKC(){Symbol = "Ground",   MinSection = 1.5,    MaxSection = 2.5,    VendorСode = "MKSGS2EARTH"},
#endregion
#region 4–6 мм2
            new CableMarkDKC(){Symbol = "0",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF0S3"},
            new CableMarkDKC(){Symbol = "1",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF1S3"},
            new CableMarkDKC(){Symbol = "2",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF2S3"},
            new CableMarkDKC(){Symbol = "3",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF3S3"},
            new CableMarkDKC(){Symbol = "4",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF4S3"},
            new CableMarkDKC(){Symbol = "5",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF5S3"},
            new CableMarkDKC(){Symbol = "6",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF6S3"},
            new CableMarkDKC(){Symbol = "7",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF7S3"},
            new CableMarkDKC(){Symbol = "8",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF8S3"},
            new CableMarkDKC(){Symbol = "9",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKF9S3"},
            new CableMarkDKC(){Symbol = "A",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCAS3"},
            new CableMarkDKC(){Symbol = "B",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCBS3"},
            new CableMarkDKC(){Symbol = "C",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCCS3"},
            new CableMarkDKC(){Symbol = "D",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCDS3"},
            new CableMarkDKC(){Symbol = "E",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCES3"},
            new CableMarkDKC(){Symbol = "F",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCFS3"},
            new CableMarkDKC(){Symbol = "G",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCGS3"},
            new CableMarkDKC(){Symbol = "H",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCHS3"},
            new CableMarkDKC(){Symbol = "I",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCIS3"},
            new CableMarkDKC(){Symbol = "J",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCJS3"},
            new CableMarkDKC(){Symbol = "K",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCKS3"},
            new CableMarkDKC(){Symbol = "L",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCLS3"},
            new CableMarkDKC(){Symbol = "M",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCMS3"},
            new CableMarkDKC(){Symbol = "N",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCNS3"},
            new CableMarkDKC(){Symbol = "O",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCOS3"},
            new CableMarkDKC(){Symbol = "P",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCPS3"},
            new CableMarkDKC(){Symbol = "Q",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCQS3"},
            new CableMarkDKC(){Symbol = "R",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCRS3"},
            new CableMarkDKC(){Symbol = "S",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCSS3"},
            new CableMarkDKC(){Symbol = "T",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCTS3"},
            new CableMarkDKC(){Symbol = "U",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCUS3"},
            new CableMarkDKC(){Symbol = "V",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCVS3"},
            new CableMarkDKC(){Symbol = "W",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCWS3"},
            new CableMarkDKC(){Symbol = "X",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCXS3"},
            new CableMarkDKC(){Symbol = "Y",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCYS3"},
            new CableMarkDKC(){Symbol = "Z",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKCZS3"},
            new CableMarkDKC(){Symbol = "Mp",       MinSection = 4,    MaxSection = 6,    VendorСode = "MKMPS3"},
            new CableMarkDKC(){Symbol = "+",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKSPS3"},
            new CableMarkDKC(){Symbol = "-",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKSMS3"},
            new CableMarkDKC(){Symbol = "*",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKSGS3"},
            new CableMarkDKC(){Symbol = "(",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKSAS3"},
            new CableMarkDKC(){Symbol = ".",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKSFS3"},
            new CableMarkDKC(){Symbol = "/",        MinSection = 4,    MaxSection = 6,    VendorСode = "MKSBS3"},
            new CableMarkDKC(){Symbol = "Ground",   MinSection = 4,    MaxSection = 6,    VendorСode = "MKSGS3EARTH"}
#endregion
        };
        private const string symbolMp = "Mp";
        private const string symbolGND = "Ground";


        public List<CableMarkDKC> GetMarksByCableName(Cable sourceCable)
        {
            sourceCable = new Cable(sourceCable);

            List<string> symbolsInCable = SplitSchemeNameToSymbols(sourceCable.SchemeName);

            List<CableMarkDKC> marks = new();

            foreach (var symbol in symbolsInCable)
            {
                IEnumerable<CableMarkDKC> findetMarks = _existingMarks.Where(x => x.Symbol == symbol);

                if (findetMarks.Count() == 0)
                    throw new SymbolNotFoundException($"Символ \"{symbol}\" не найден в каталоге");

                foreach (var item in findetMarks)
                {
                    if (item.MaxSection > sourceCable.WireSection && item.MinSection <= sourceCable.WireSection)
                        for (int i = 0; i < sourceCable.WireCount; i++)
                            marks.Add(new CableMarkDKC(item));
                }
            }

            return marks;
        }

        public List<string> SplitSchemeNameToSymbols(string schemeName)
        {
            List<string> symbolsInCable = new();

            if (schemeName.Contains(symbolMp))
            {
                symbolsInCable.Add(symbolMp);
                schemeName = schemeName.Replace(symbolMp, String.Empty);
            }

            if (schemeName.Contains(symbolGND))
            {
                symbolsInCable.Add(symbolGND);
                schemeName = schemeName.Replace(symbolGND, String.Empty);
            }

            foreach (char symbol in schemeName)
                symbolsInCable.Add(symbol.ToString());

            return symbolsInCable;
        }
    }
}
