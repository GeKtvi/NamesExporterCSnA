using System.Reflection;

namespace NamesExporterCSnA.Data
{
    public abstract class FullNameBase
    {
        public abstract string FullName { get; }

        protected string GetFullName(string nameTemplate, PropertyInfo[] props)
        {
            foreach (PropertyInfo prop in props)
                if (prop.Name != nameof(FullName))
                {
                    string name = '{' + prop.Name + '}';
                    object propVal = prop.GetValue(this);
                    string propValS = propVal?.ToString() ?? throw new InvalidDataException("Invalid value in property in heir of FullNameBase");
                    nameTemplate = nameTemplate.Replace(name, propValS);
                }

            return nameTemplate;
        }
    }
}
