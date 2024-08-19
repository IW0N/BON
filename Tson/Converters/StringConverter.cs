using System.Collections;
using System.Text;
using Tson.Options;

namespace Tson.Converters
{
    public class StringConverter:TsonConverter<string>
    {
        private TsonConverter<byte[]> _converter;

        public override void Init(TsonOptions options)
        {
            _converter = TsonSerializer.GetConverter<byte[]>(options);
        }

        public override string Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var len = reader.ReadDataLength();
            var bytes = reader.ReadBytes(len).ToArray();
            return Encoding.UTF8.GetString(bytes);
        }

        public override void Write(TsonWriter writer, string data, TsonContext context)
        {
            var bts = Encoding.UTF8.GetBytes(data);
            
            _converter.Write(writer, bts, context);
        }
    }
}
