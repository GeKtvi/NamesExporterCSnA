using System.Xml.Serialization;

namespace NamesExporterCSnA.Data.Cables
{
    [XmlInclude(typeof(ColorMapper))]
    public class CableTemplate
    {
        public string SubCableType { get; set; } = "{NotSet}";
        public string FullCableType { get; set; } = "{NotSet}";
        public string Template { get; set; } = "{NotSet}";
        public bool HasFixedLength { get; set; }
        public double Length { get; set; }
        public bool HasColor { get; set; } = false;

        public string ParseOutType { get; set; } = nameof(Cable);

        public ColorMapper _colorMapper;
        public ColorMapper ColorMapper
        {
            get
            {
                return HasColor == false
                    ? throw new InvalidOperationException(
                        $"Кабель не имеет, цвета невозможно получить {nameof(ColorMapper)}, " +
                        $"необходимо установит {nameof(HasColor)} в true")
                    : _colorMapper;
            }
            set
            {
                if (HasColor == false)
                    throw new InvalidOperationException(
                        $"Кабель не имеет, цвета невозможно задать {nameof(ColorMapper)}, " +
                        $"необходимо установит {nameof(HasColor)} в true");
                _colorMapper = value;
            }
        }
    }
}
