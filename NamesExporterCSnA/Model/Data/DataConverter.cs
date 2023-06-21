using NamesExporterCSnA.Model.Data.Marks;
using NamesExporterCSnA.Model.Data.Cables;
using NamesExporterCSnA.Services.UpdateLog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using NamesExporterCSnA.Services.Settings;

namespace NamesExporterCSnA.Model.Data
{
    public class DataConverter
    {
        public CablesParser CablesParser { get; private set; }
        public CableMarkFactory CableMarkDKCFabric { get; private set; }

        public IUpdateLogger Logger { get; private set; }

        public DataConverter(IUpdateLogger logger, IPreferencesSettings settings)
        {
            Logger = logger;
            CablesParser = new(Logger);
            CableMarkDKCFabric = new(Logger, settings);
        }

        public List<IDisplayableData> Convert(List<MaxExportedCable> cables)
        {
            List<IDisplayableData> displayableData = new List<IDisplayableData>();
            Logger.ClearLog();
            try
            {
                var parsed = CablesParser.Parse(cables);

                if (parsed.Count == 0)
                    return displayableData;

                List<ICableMark> marks = new();

                foreach (var cable in parsed)
                    marks.AddRange(CableMarkDKCFabric.CreateMarksForCable(cable));

                displayableData.AddRange(ConvertToIDisplayableData<DisplayableCable, Cable>(parsed));
                displayableData.AddRange(ConvertToIDisplayableData<DisplayableCableMark, ICableMark>(marks));
                return displayableData;
            }
            catch (Exception ex)
            {
                Logger.Log(
                        new UpdateFail()
                        {
                            Message = $"Необрабатываемая ошибка: {ex.Message}",
                            Type = UpdateFailType.Error,
                            SchemeName = "-",
                            WireName = "-",
                            Source = "Модуль конвертации"
                        }
                    );
                return new List<IDisplayableData>();
            }
        }

        private List<IDisplayableData> ConvertToIDisplayableData<DisplayableObj, GroupingObj>(List<GroupingObj> marks)
            where DisplayableObj : class, IFromGroup<GroupingObj>, new()
            where GroupingObj : IFullName
        {
            List<IDisplayableData> groupedList = new();

            var groupedMarks =
            from mark in marks
            group mark by mark.FullName into newGroup
            orderby newGroup.Key
            select newGroup;

            foreach (var item in groupedMarks)
                groupedList.Add(new DisplayableObj().SetFromGrouping(item));

            return groupedList;
        }
    }
}
