using NamesExporterCSnA.Data.Cables;
using NamesExporterCSnA.Data.Marks;
using NamesExporterCSnA.Data.Marks.Exceptions;
using NamesExporterCSnA.Data.Settings;
using NamesExporterCSnA.Data.UpdateLog;
using System.Data;

namespace NamesExporterCSnA.Data
{
    public class DataConverter
    {
        public CablesParser CablesParser { get; private set; }
        public CableMarkFactory CableMarkDKCFabric { get; private set; }

        public event Action SettingsChanged
        {
            add { _settings.DataConverterSettingChanged += value; }
            remove { _settings.DataConverterSettingChanged -= value; }
        }
        public IUpdateLogger Logger { get; private set; }

        private IPreferencesSettings _settings;

        public DataConverter(
            IUpdateLogger logger,
            IPreferencesSettings settings,
            CablesParserConfig config,
            CableMarkVendorData[] cableMarkVendorsData,
            CableMarkingWhiteList cableForMarkingWhiteList)
        {
            Logger = logger;
            _settings = settings;

            CablesParser = new(Logger, _settings.ApproximateCableLength, config);
            CableMarkDKCFabric = new(Logger, _settings, cableMarkVendorsData, cableForMarkingWhiteList);
        }

        public List<IDisplayableData> Convert(List<MaxExportedCable> cables)
        {
            List<IDisplayableData> displayableData = new List<IDisplayableData>();
            Logger.FreezeLogNotify();
            Logger.ClearLog();
            try
            {
                List<ICable> parsed = CablesParser.Parse(cables);

                System.Collections.Concurrent.ConcurrentBag<ICableMark> marks = new();

                parsed.AsParallel().ForAll(cable =>
                    {
                        foreach (ICableMark mark in CableMarkDKCFabric.CreateMarksForCable(cable))
                            marks.Add(mark);
                    }
                );

                displayableData.AddRange(ConvertToIDisplayableData<DisplayableCable, ICable>(parsed));
                displayableData.AddRange(ConvertToIDisplayableData<DisplayableCableMark, ICableMark>(marks.ToList()));
            }
            catch (Exception)
            {
#if !DEBUG
                Logger.Log(
                        new UpdateFail()
                        {
                            Message = $"Не обрабатываемая ошибка: {ex.Message}",
                            Type = UpdateFailType.Error,
                            SchemeName = "-",
                            WireName = "-",
                            Source = "Модуль конвертации"
                        }
                    );
                displayableData =  new List<IDisplayableData>();
#else
                throw;
#endif
            }
            Logger.UnfreezeLogNotify();
            return displayableData;
        }

        private static List<IDisplayableData> ConvertToIDisplayableData<DisplayableObj, GroupingObj>(List<GroupingObj> objects)
            where DisplayableObj : class, IFromGroup<GroupingObj>, new()
            where GroupingObj : IFullName
        {
            List<IDisplayableData> groupedList = new();

            OrderedParallelQuery<IGrouping<string, GroupingObj>> groupedMarks =
            from obj in objects.AsParallel()
            group obj by obj.FullName into newGroup
            orderby newGroup.Key
            select newGroup;

            foreach (IGrouping<string, GroupingObj> item in groupedMarks)
                groupedList.Add(new DisplayableObj().SetFromGrouping(item));

            return groupedList.ToList();
        }
    }
}
