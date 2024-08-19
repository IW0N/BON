
using System.Runtime.InteropServices;
using Tson.Converters;
using Tson.Options;

namespace Tson.Serializers
{
    public class IntConverter : TsonConverter<int>
    {
        public override int Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var data = reader.ReadBytes(sizeof(int));
            var result = 0;
            var shift = 0;
            
            foreach (byte b in data)
            {
                result |= b << shift;
                shift += 8;
            }

            return result;
        }

        public override void Write(TsonWriter writer, int data, TsonContext context)
        {
            byte[] intBytes = BitConverter.GetBytes(data);
            writer.WriteBytes(intBytes);
        }
    }
}
