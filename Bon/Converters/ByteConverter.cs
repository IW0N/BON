using Bon.Options;

namespace Bon.Converters
{
    public class ByteConverter : BonConverter<byte>
    {
        public override byte Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var success = reader.ReadByte(out byte result);
            if(!success)
            {
                throw new OverflowException();
            }
            return result;
        }

        public override void Write(BonWriter writer, byte data, BonContext context)
        {
            writer.WriteByte(data);

            Inited = true;
        }
    }
}
