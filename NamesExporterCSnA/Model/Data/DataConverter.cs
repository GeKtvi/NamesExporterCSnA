using NamesExporterCSnA.Model.Data.Marks;
using NamesExporterCSnA.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data
{
    public class DataConverter
    {
        public CablesParser CablesParser { get; private set; }
        public CableMarkFactory CableMarkDKCFabric { get; private set; } = new CableMarkFactory();

        public IUpdateLogger Logger { get; private set; }

        public DataConverter(IUpdateLogger logger) 
        {
            Logger = logger;
            CablesParser = new(Logger);
        }  

        public List<DisplayableDataOut> Convert(List<MaxExportedCable> cables) 
        {
            Logger.ClearLog();
            try
            {
                var parsed = CablesParser.Parse(cables);

                if (parsed.Count == 0)
                    return new List<DisplayableDataOut>();

                List<ICableMark> marks = new();

                foreach (var cable in parsed)
                    marks.AddRange(CableMarkDKCFabric.CreateMarksForCable(cable));

                return GroupICableMark(marks);
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
                return new List<DisplayableDataOut>();
            }
        }

        private List<DisplayableDataOut> GroupICableMark(List<ICableMark> marks)
        {
            List<DisplayableDataOut> groupedList = new(); 

            var groupedMarks =
            from mark in marks
            group mark by mark.FullName into newGroup
            orderby newGroup.Key
            select newGroup;

            foreach (var item in groupedMarks)
                groupedList.Add(new DisplayableDataOut(item));

            return groupedList;
        }
    }
}
