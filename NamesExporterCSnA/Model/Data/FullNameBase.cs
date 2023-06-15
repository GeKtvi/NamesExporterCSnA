using System.Reflection;

namespace NamesExporterCSnA.Model.Data
{
    public abstract class FullNameBase
    {
        public abstract string FullName { get; }

        protected string GetFullName(string nameTemplate, PropertyInfo[] props) 
        {
            foreach (var prop in props)
                if (prop.Name != nameof(FullName))
                    nameTemplate = nameTemplate.Replace('{' + prop.Name + '}', prop.GetValue(this).ToString());

            return nameTemplate;
        }
    }
}
