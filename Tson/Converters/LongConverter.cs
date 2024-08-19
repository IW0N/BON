using System.Runtime.InteropServices;
using Tson.Options;

namespace Tson.Converters
{
    public class LongConverter : TsonConverter<long>
    {
        public override long Read(TsonReader reader, Type typeToConvert, TsonContext context)
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

        public override void Write(TsonWriter writer, long data, TsonContext context)
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
