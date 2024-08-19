using Tson.Options;

namespace Tson.Converters
{
    public class ShortConverter : TsonConverter<short>
    {
        public override short Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            reader.ReadByte(out byte a);
            reader.ReadByte(out byte b);
            short result = (short)(a | b << 8);
            return result;
        }

        public override void Write(TsonWriter writer, short data, TsonContext context)
        {
            writer.WriteByte((byte)data);
            writer.WriteByte((byte)(data>>8));
        }
    }
}
