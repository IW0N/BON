using System.Collections;
using System.Text.Json;
using Bon.Converters;
using Bon.Options;
using Bon.Serializers;

namespace Bon
{
    public static class BonSerializer
    {
        internal static BonOptions RootOptions { get; private set; }
        internal static bool Inited { get; private set; }

        public static byte[] Serialize<T>(T data, BonOptions? options = null)
        {
            options ??= RootOptions;

            var dataType = data.GetType();
            var converter = GetConverter(dataType, options);

            if (converter is null)
            {
                throw new Exception("Unknown type!");
            }

            var context = new BonContext { Index = 0, Options = options };

            var writer = new BonWriter([], context);

            converter.BaseWrite(writer, data, context);

            return writer.ToByteArray();
        }
        public static T Deserialize<T>(byte[] data, BonOptions? options = null)
        {
            options ??= RootOptions;
            
            var converter = GetConverter(typeof(T), options);
            
            var context = new BonContext { Index = 0, Options = options };

            var reader = new BonReader(data, context);

            return (T)converter.BaseRead(reader, typeof(T), context);
        }
        public static BonConverter GetConverter(Type dataType, BonOptions? options = null)
        {
            var convertible = (options ?? RootOptions).Converters.FirstOrDefault(c => c.CanConvert(dataType));
            if (convertible is null)
            {
                throw new Exception($"Unknown converter for type {dataType}");
            }
            else if(convertible is BonConverter converter)
            {
                return converter;
            }
            else if (convertible is BonConverterFactory factory)
            {
                return factory.BuildConverter(dataType, options);
            }

            throw new Exception("WTF?!");
        }

        public static BonConverter<T> GetConverter<T>(BonOptions? options = null) =>
            (BonConverter<T>)GetConverter(typeof(T), options);

        //it invokes static constructor if not inited
        internal static void Init()
        {
            if (Inited)
            {
                return;
            }

            IEnumerable<IBonConvertible> convertibles =
            [
                new ByteConverter(),
                new PlainFixedConverter<byte,bool>(),
                new PlainFixedConverter<byte,char>(),
                new PlainFixedConverter<byte,sbyte>(),

                new ShortConverter(),
                new PlainFixedConverter<short,ushort>(),

                new IntConverter(),
                new PlainFixedConverter<int,uint>(),
                new PlainFixedConverter<int,float>(),

                new LongConverter(),
                new PlainFixedConverter<long,ulong>(),
                new PlainFixedConverter<long,double>(),
                new PlainFixedConverter<long,DateTime>(),

                new ArrayConverterFactory(),

                new DecimalConverter(),
                new PlainFixedConverter<decimal,Guid>(),

                new EnumConverter(),

                new ObjectConverter(),

                new StringConverter(),

                new ListConverterFactory(),

                new DictionaryConverterFactory(),
                new KeyValuePairConverterFactory(),

                new NullableConverterFactory()
            ];

            RootOptions = new(LengthSize.UInt16, new ConverterList(convertibles));
        }
    }
}
