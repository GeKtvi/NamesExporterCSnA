#pragma warning disable CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)
#pragma warning disable CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
namespace NamesExporterCSnA.Data.Cables
{
    [Equals]
    public class Cable : FullNameBase, IFullName, ICable
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
        public double Length
        {
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
        public string Color
        {
            get
            {
                return HasColor == false ? "{The cable has no color}" : _color;
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

        public static bool operator ==(Cable left, Cable right) => Operator.Weave(left, right);
        public static bool operator !=(Cable left, Cable right) => Operator.Weave(left, right);
    }
}

#pragma warning restore CS0661 // Type defines operator == or operator != but does not override Object.GetHashCode()
#pragma warning restore CS0660 // Type defines operator == or operator != but does not override Object.Equals(object o)