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
                {
                    //TODO optimize
                    var name = '{' + prop.Name + '}';
                    var propVal = prop.GetValue(this);
                    string propValS = propVal.ToString();
                    nameTemplate = nameTemplate.Replace(name, propValS);
                }

            return nameTemplate;
        }
    }
}
