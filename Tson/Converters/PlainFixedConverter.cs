using System.Runtime.CompilerServices;
using Tson.Options;

namespace Tson.Converters
{
    public class PlainFixedConverter<TBase,TRes>:TsonConverter<TRes> where TBase:struct where TRes:struct
    {
        private TsonConverter<TBase> _baseConverter;

        public override void Init(TsonOptions options)
        {
            _baseConverter = TsonSerializer.GetConverter<TBase>(options);
        }

        public override TRes Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var @base = _baseConverter.Read(reader, typeToConvert, context);
            return Unsafe.BitCast<TBase, TRes>(@base);
        }

        public override void Write(TsonWriter writer, TRes data, TsonContext context)
        {
            var @base = Unsafe.BitCast<TRes,TBase>(data);
            _baseConverter.Write(writer, @base, context);
        }
    }
}
