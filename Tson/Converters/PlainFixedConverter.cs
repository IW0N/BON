using System.Runtime.CompilerServices;
using Bon.Options;

namespace Bon.Converters
{
    public class PlainFixedConverter<TBase,TRes>:BonConverter<TRes> where TBase:struct where TRes:struct
    {
        private BonConverter<TBase> _baseConverter;

        public override void Init(BonOptions options)
        {
            _baseConverter = BonSerializer.GetConverter<TBase>(options);
        }

        public override TRes Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var @base = _baseConverter.Read(reader, typeToConvert, context);
            return Unsafe.BitCast<TBase, TRes>(@base);
        }

        public override void Write(BonWriter writer, TRes data, BonContext context)
        {
            var @base = Unsafe.BitCast<TRes,TBase>(data);
            _baseConverter.Write(writer, @base, context);
        }
    }
}
