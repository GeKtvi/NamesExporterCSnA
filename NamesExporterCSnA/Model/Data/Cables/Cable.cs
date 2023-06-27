using NamesExporterCSnA.Model.Data.Marks;
using System;

namespace NamesExporterCSnA.Model.Data.Cables
{
    public class Cable : FullNameBase, IFullName
    {
        public string CableType { get; set; } = "{NotSet}";
        public string SchemeName { get; set; } = "{NotSet}";
        public double WireSection { get; set; } = 0;
        public int WireCount { get; set; } = 0;
        public int WirePairs { get; set; } = 0;
        public string NormativeDocument { get; set; } = "{NotSet}";
        public string Template { get; set; } = "{NotSet}";

        public bool HasFixedLength { get; set; } = false;
        private double _length = 0;
        public double Length { 
            get => _length;
            set
            {
                if (HasFixedLength == true)
                    throw new InvalidOperationException("Нельзя установить длину кабеля с фиксированной длиной");
                _length = value;
            } 
        }

        public bool HasColor { get; set; } = false;
        private string _color = "{NotSet}";
        public string Color { 
            get
            {
                if (HasColor == false)
                    return "{The cable has no color}";
                return _color;
            }
            set
            {
                if (HasColor == false)
                    throw new InvalidOperationException("Кабель не имеет цвета");
                _color = value;
            }
        }

        public override string FullName => GetFullName(Template, PropertyHolder<Cable>.GetProperties());

        public Cable() { }

        public Cable(string cableType, string schemeName, double wireSection, int wireCount, string normativeDocument)
        {
            CableType = cableType;
            SchemeName = schemeName;
            WireSection = wireSection;
            WireCount = wireCount;
            NormativeDocument = normativeDocument;
        }

        public Cable(Cable cable)
        {
            CableType = cable.CableType;
            SchemeName = cable.SchemeName;
            WireSection = cable.WireSection;
            WireCount = cable.WireCount;
            NormativeDocument = cable.NormativeDocument;
        }

        public override bool Equals(object obj)
        {
            Cable objectToCompare;
            if (obj is Cable cable)
                objectToCompare = cable;
            else
                return false;

            bool isEqual = true;

            foreach (System.Reflection.PropertyInfo prop in PropertyHolder<Cable>.GetProperties())
                if (prop.GetType() != typeof(Cable) &&
                    prop.GetValue(this).Equals(prop.GetValue(objectToCompare)) == false)
                    isEqual = false;

            return isEqual;
        }
    }
}
