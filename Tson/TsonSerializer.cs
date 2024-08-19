using System.Collections;
using System.Text.Json;
using Tson.Converters;
using Tson.Options;
using Tson.Serializers;

namespace Tson
{
    public static class TsonSerializer
    {
        internal static readonly TsonOptions rootOptions;

        static TsonSerializer()
        {
            IEnumerable<ITsonConvertible> convertibles =
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
                
                new ListConverterFactory()
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

        public static byte[] Serialize<T>(T data, TsonOptions? options = null)
        {
            var concated = ConcatOptions(options);
            var dataType = data.GetType();
            var converter = GetConverter(dataType, concated);

            if (converter is null)
            {
                //todo: make default ObjectConverter for this way
                throw new Exception("Unknown type!");
            }
            var context = new TsonContext { Index = 0, Options = concated };

            var writer = new TsonWriter([], context);

            converter.BaseWrite(writer, data, context);

            return writer.ToByteArray();
        }
        public static T Deserialize<T>(byte[] data, TsonOptions? options = null)
        {
            var concated = ConcatOptions(options);
            var converter = GetConverter(typeof(T), concated);

            var context = new TsonContext { Index = 0, Options = concated };

            var reader = new TsonReader(data, context);

            return (T)converter.BaseRead(reader, typeof(T), context);
        }
        public static TsonConverter GetConverter(Type dataType, TsonOptions? options = null)
        {
            var convertible = (options ?? rootOptions).Converters.FirstOrDefault(c => c.CanConvert(dataType));
            if (convertible is null)
            {
                throw new Exception($"Unknown converter for type {dataType}");
            }
            else if(convertible is TsonConverter converter)
            {
                return converter;
            }
            else if (convertible is TsonConverterFactory factory)
            {
                return factory.BuildConverter(dataType, options);
            }

            throw new Exception("WTF?!");
        }

        public static TsonConverter<T> GetConverter<T>(TsonOptions? options = null) =>
            (TsonConverter<T>)GetConverter(typeof(T), options);

        private static TsonOptions ConcatOptions(TsonOptions? userOptions)
        {
            if(userOptions is null)
            {
                return rootOptions;
            }
            else
            {
                var opt = new TsonOptions()
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
