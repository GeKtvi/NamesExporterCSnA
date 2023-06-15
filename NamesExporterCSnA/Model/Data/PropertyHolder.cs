using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace NamesExporterCSnA.Model.Data
{
    internal class PropertyHolder<T>
    {
        private static PropertyInfo[] _properties;

        public static PropertyInfo[] GetProperties()
        {
            if (_properties is null)
                _properties = typeof(T).GetProperties().ToArray();  
            return _properties;
        }
    }
}
