using NamesExporterCSnA.Model.Data.Marks;
using System;
using System.Collections.Generic;
using System.Data;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data
{
    internal class DataConverter
    {
        public CablesParser CablesParser { get; private set; } = new();
        public CableMarkFactory CableMarkDKCFabric { get; private set; } = new CableMarkFactory();

        public DataConverter() { }

        public List<DisplayableDataOut> Convert(List<MaxExportedCable> cables) 
        {
            var parsed = CablesParser.Parse(cables);

            if (parsed.Count == 0)
                return new List<DisplayableDataOut>();


            List<ICableMark> marks = new();

            foreach (var cable in parsed)
                marks.AddRange(CableMarkDKCFabric.CreateMarksForCable(cable));

            return GroupICableMark(marks);
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
