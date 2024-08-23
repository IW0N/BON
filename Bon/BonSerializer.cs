using System.Collections;
using System.Text.Json;
using Bon.Converters;
using Bon.Options;
using Bon.Serializers;

namespace Bon
{
    public static class BonSerializer
    {
        internal static readonly BonOptions rootOptions;

        static BonSerializer()
        {
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

            rootOptions = new()
            {
                DataArrayLengthSize = LengthSize.UInt16,
                Converters = new ConverterList(convertibles)
            };

            foreach (var c in rootOptions.Converters)
            {
                c.Init(rootOptions);
            }
        }

        public static byte[] Serialize<T>(T data, BonOptions? options = null)
        {
            var concated = ConcatOptions(options);
            var dataType = data.GetType();
            var converter = GetConverter(dataType, concated);

            if (converter is null)
            {
                throw new Exception("Unknown type!");
            }

            var context = new BonContext { Index = 0, Options = concated };

            var writer = new BonWriter([], context);

            converter.BaseWrite(writer, data, context);

            return writer.ToByteArray();
        }
        public static T Deserialize<T>(byte[] data, BonOptions? options = null)
        {
            var concated = ConcatOptions(options);
            var converter = GetConverter(typeof(T), concated);

            var context = new BonContext { Index = 0, Options = concated };

            var reader = new BonReader(data, context);

            return (T)converter.BaseRead(reader, typeof(T), context);
        }
        public static BonConverter GetConverter(Type dataType, BonOptions? options = null)
        {
            var convertible = (options ?? rootOptions).Converters.FirstOrDefault(c => c.CanConvert(dataType));
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

        private static BonOptions ConcatOptions(BonOptions? userOptions)
        {
            if(userOptions is null)
            {
                return rootOptions;
            }
            else
            {
                var opt = new BonOptions()
                {
                    DataArrayLengthSize = userOptions.DataArrayLengthSize,
                    Converters = new ConverterList(userOptions.Converters)
                };
                opt.Converters.AddRange(userOptions.Converters);
                return opt;
            }
        }
    }
}
