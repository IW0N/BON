using Bon.Options;

namespace Bon.Converters
{
    public class KeyValuePairConverterFactory : BonConverterFactory
    {
        public override Type Type => typeof(KeyValuePair<,>);

        public override BonConverter BuildConverter(Type outputType, BonOptions options)
        {
            var generics = GetGenericTypes(outputType);
            var type = typeof(KeyValuePairConverter<,>).MakeGenericType(generics);
            var conv = (BonConverter)Activator.CreateInstance(type);
            conv.Init(options);
            return conv;
        }

        public override bool CanConvert(Type typeToConvert)
        {
            var generics = GetGenericTypes(typeToConvert);
            return generics.Length > 0;
        }

        private Type[] GetGenericTypes(Type typeToConvert)
        {
            var type = typeToConvert;

            while (type != null && type != Type)
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == Type)
                {
                    type = typeToConvert;
                    break;
                }

                type = type.BaseType;
            }

            return type?.GenericTypeArguments ?? [];
        }
    }

    public class KeyValuePairConverter<TKey,TVal>:BonConverter<KeyValuePair<TKey,TVal>>
    {
        private BonConverter<TKey> _keyConverter;
        private BonConverter<TVal> _valConverter;

        public override void Init(BonOptions options)
        {
            _keyConverter = BonSerializer.GetConverter<TKey>(options);
            _valConverter = BonSerializer.GetConverter<TVal>(options);
            _keyConverter.Init(options);
            _valConverter.Init(options);
        }

        public override KeyValuePair<TKey, TVal> Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var key = _keyConverter.Read(reader, typeToConvert, context);
            var value = _valConverter.Read(reader,typeToConvert,context);
            return new KeyValuePair<TKey, TVal>(key, value);
        }

        public override void Write(BonWriter writer, KeyValuePair<TKey, TVal> data, BonContext context)
        {
            _keyConverter.Write(writer, data.Key, context);
            _valConverter.Write(writer, data.Value, context);
        }
    }
}
