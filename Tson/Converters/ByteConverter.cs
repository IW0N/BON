using Tson.Options;

namespace Tson.Converters
{
    public class ByteConverter : TsonConverter<byte>
    {
        public override byte Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var success = reader.ReadByte(out byte result);
            if(!success)
            {
                throw new OverflowException();
            }
            return result;
        }

        public override void Write(TsonWriter writer, byte data, TsonContext context)
        {
            writer.WriteByte(data);
        }
    }
}
