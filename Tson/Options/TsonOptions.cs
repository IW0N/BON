using Tson.Converters;

namespace Tson.Options
{
    public class TsonOptions
    {
        public LengthSize DataArrayLengthSize { get; set; } = LengthSize.UInt16;

        public ConverterList Converters { get; }

        public TsonOptions()
        {
            Converters = new ConverterList(this);
        }
        internal TsonOptions(ConverterList list)
        {
            Converters = list;
        }
    }
}