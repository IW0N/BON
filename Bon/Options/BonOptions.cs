using Bon.Converters;

namespace Bon.Options
{
    public class BonOptions
    {
        public LengthSize DataArrayLengthSize { get; set; } = LengthSize.UInt16;

        public ConverterList Converters { get; internal set; }
    }
}