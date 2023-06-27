using NamesExporterCSnA.Model.Data.Cables;
using NamesExporterCSnA.Model.Data.Marks;
using NamesExporterCSnA.Services.Settings;
using NamesExporterCSnA.Services.UpdateLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NamesExporterCSnA.Model.Data
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

        public DataConverter(IUpdateLogger logger, IPreferencesSettings settings)
        {
            Logger = logger;
            _settings = settings;

            CablesParser = new(Logger, _settings.ApproximateCableLength);
            CableMarkDKCFabric = new(Logger, _settings);
        }

        public List<IDisplayableData> Convert(List<MaxExportedCable> cables)
        {
            List<IDisplayableData> displayableData = new List<IDisplayableData>();
            Logger.FreezeLogNotify();
            Logger.ClearLog();
            try
            {
                List<Cable> parsed = CablesParser.Parse(cables);

                List<ICableMark> marks = new();

                foreach (Cable cable in parsed)
                    marks.AddRange(CableMarkDKCFabric.CreateMarksForCable(cable));

                displayableData.AddRange(ConvertToIDisplayableData<DisplayableCable, Cable>(parsed));
                displayableData.AddRange(ConvertToIDisplayableData<DisplayableCableMark, ICableMark>(marks));
            }
            catch (Exception ex)
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

        private List<IDisplayableData> ConvertToIDisplayableData<DisplayableObj, GroupingObj>(List<GroupingObj> marks)
            where DisplayableObj : class, IFromGroup<GroupingObj>, new()
            where GroupingObj : IFullName
        {
            List<IDisplayableData> groupedList = new();

            IOrderedEnumerable<IGrouping<string, GroupingObj>> groupedMarks =
            from mark in marks
            group mark by mark.FullName into newGroup
            orderby newGroup.Key
            select newGroup;

            foreach (IGrouping<string, GroupingObj> item in groupedMarks)
                groupedList.Add(new DisplayableObj().SetFromGrouping(item));

            return groupedList;
        }
    }
}
