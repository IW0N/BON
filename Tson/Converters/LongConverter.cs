using System.Runtime.InteropServices;
using Bon.Options;

namespace Bon.Converters
{
    public class LongConverter : BonConverter<long>
    {
        public override long Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var data = reader.ReadBytes(sizeof(long));
            long result = 0;
            var shift = 0;

            foreach (long b in data)
            {
                result |= b << shift;
                shift += 8;
            }

            return result;
        }

        public override void Write(BonWriter writer, long data, BonContext context)
        {
            long mask = 0xff;
            for (int i = 0; i < sizeof(long); i++)
            {
                var b = (data & mask) >> (i * 8);
                writer.WriteByte((byte)b);
                mask <<= 8;
            }
        }
    }
}
