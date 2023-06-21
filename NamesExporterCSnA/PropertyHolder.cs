using System.Linq;
using System.Reflection;

namespace NamesExporterCSnA
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

        public static void SetPropertiesValue(T objToSet, T objSetFrom)
        {
            foreach (var prop in GetProperties())
                prop.SetValue(objToSet, prop.GetValue(objSetFrom));
        }
    }
}
