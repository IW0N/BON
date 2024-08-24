using System.Runtime.CompilerServices;
using Bon.Options;

namespace Bon.Converters
{
    /// <summary>
    /// Converter, that allow convert <see cref="TBase"/> into <see cref="TRes"/>
    /// <br/>if byte size of <see cref="TBase"/> is same as <see cref="TRes"/>
    /// <br/>
    /// </summary>
    /// <typeparam name="TBase"></typeparam>
    /// <typeparam name="TRes"></typeparam>
    public class PlainFixedConverter<TBase,TRes>:BonConverter<TRes> where TBase:struct where TRes:struct
    {
        private BonConverter<TBase> _baseConverter;

        public override void Init(BonOptions options)
        {
            _baseConverter = BonSerializer.GetConverter<TBase>(options);
            Inited = true;
        }
        /// <summary>
        /// Converter takes TBase from byte array,<br/> 
        /// creates new instance of TRes and then transmitts bits from TBase to TRes
        /// </summary>
        /// <param name="reader"></param>
        /// <param name="typeToConvert"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public override TRes Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var @base = _baseConverter.Read(reader, typeToConvert, context);
            return Unsafe.BitCast<TBase, TRes>(@base);
        }

        /// <summary>
        /// Converter casts bits from TRes to TBase, and then invokes converter for TBase
        /// </summary>
        /// <param name="writer"></param>
        /// <param name="data"></param>
        /// <param name="context"></param>
        public override void Write(BonWriter writer, TRes data, BonContext context)
        {
            var @base = Unsafe.BitCast<TRes,TBase>(data);
            _baseConverter.Write(writer, @base, context);
        }
    }
}
