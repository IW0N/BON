using System.Collections;
using System.Text;
using Bon.Options;

namespace Bon.Converters
{
    public class StringConverter:BonConverter<string>
    {
        private BonConverter<byte[]> _converter;

        public override void Init(BonOptions options)
        {
            _converter = BonSerializer.GetConverter<byte[]>(options);
        }

        public override string Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var len = reader.ReadDataLength();
            var bytes = reader.ReadBytes(len).ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        public override void Write(BonWriter writer, string data, BonContext context)
        {
            var bts = Encoding.UTF8.GetBytes(data);
            
            _converter.Write(writer, bts, context);
        }
    }
}
