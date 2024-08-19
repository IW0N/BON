using Bon.Options;

namespace Bon.Converters
{
    public class ShortConverter : BonConverter<short>
    {
        public override short Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            reader.ReadByte(out byte a);
            reader.ReadByte(out byte b);
            short result = (short)(a | b << 8);
            return result;
        }

        public override void Write(BonWriter writer, short data, BonContext context)
        {
            writer.WriteByte((byte)data);
            writer.WriteByte((byte)(data>>8));
        }
    }
}
