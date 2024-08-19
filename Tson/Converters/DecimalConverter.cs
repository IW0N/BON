using System.Runtime.CompilerServices;
using Tson.Options;

namespace Tson.Converters
{
    struct BinDecimal
    {
        public long a;
        public long b;
    }

    public class DecimalConverter : TsonConverter<decimal>
    {
        private TsonConverter<long> _converter;
        public override void Init(TsonOptions options)
        {
            _converter = TsonSerializer.GetConverter<long>(options);
        }

        public override decimal Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var a = _converter.Read(reader, typeToConvert, context);
            var b = _converter.Read(reader,typeToConvert,context);
            var c = new BinDecimal { a = a, b = b };
            var result = Unsafe.BitCast<BinDecimal,decimal>(c);
            return result;
        }

        public override void Write(TsonWriter writer, decimal data, TsonContext context)
        {
            var c = Unsafe.BitCast<decimal,BinDecimal>(data);
            _converter.Write(writer, c.a, context);
            _converter.Write(writer, c.b, context);
        }
    }
}