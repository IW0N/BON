
using System.Runtime.InteropServices;
using Bon.Converters;
using Bon.Options;

namespace Bon.Serializers
{
    public class IntConverter : BonConverter<int>
    {
        public override int Read(BonReader reader, Type typeToConvert, BonContext context)
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

        public override void Write(BonWriter writer, int data, BonContext context)
        {
            byte[] intBytes = BitConverter.GetBytes(data);
            writer.WriteBytes(intBytes);
        }
    }
}
