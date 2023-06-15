using NamesExporterCSnA.Model.Data.Cables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NamesExporterCSnA.Model.Data
{
    public interface IFromGroup<T>
    {
        IDisplayableData SetFromGrouping(IGrouping<string, T> group);
    }
}
