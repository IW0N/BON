using System.Runtime.CompilerServices;
using Bon.Options;

namespace Bon.Converters
{
    struct BinDecimal
    {
        public long a;
        public long b;
    }

    public class DecimalConverter : BonConverter<decimal>
    {
        private BonConverter<long> _converter;
        public override void Init(BonOptions options)
        {
            _converter = BonSerializer.GetConverter<long>(options);
        }

        public override decimal Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var a = _converter.Read(reader, typeToConvert, context);
            var b = _converter.Read(reader,typeToConvert,context);
            var c = new BinDecimal { a = a, b = b };
            var result = Unsafe.BitCast<BinDecimal,decimal>(c);
            return result;
        }

        public override void Write(BonWriter writer, decimal data, BonContext context)
        {
            var c = Unsafe.BitCast<decimal,BinDecimal>(data);
            _converter.Write(writer, c.a, context);
            _converter.Write(writer, c.b, context);
        }
    }
}