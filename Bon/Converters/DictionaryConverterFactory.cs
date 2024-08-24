using Bon.Options;
using System.Collections;

namespace Bon.Converters
{
    public class DictionaryConverterFactory : BonConverterFactory
    {
        public override Type Type => typeof(Dictionary<,>);

        public override BonConverter BuildConverter(Type outputType, BonOptions options)
        {
            var generics = GetGenericTypes(outputType);
            var type = typeof(DictionaryConverter<,>).MakeGenericType(generics);
            var converter= (BonConverter)Activator.CreateInstance(type);
            converter.Init(options);
            return converter;
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

            return type?.GenericTypeArguments??[];
        }
    }
    public class DictionaryConverter<TKey, TVal> : EnumerableConverter<Dictionary<TKey, TVal>> where TKey : notnull
    {
        private KeyValuePairConverter<TKey, TVal> _kvpConverter;
        public override void Init(BonOptions options)
        {
            _kvpConverter = (KeyValuePairConverter<TKey, TVal>)BonSerializer.GetConverter<KeyValuePair<TKey,TVal>>(options);
            Inited = true;
        }

        protected override Dictionary<TKey, TVal> Cast(IList source, Type valType)
        {
            var dict = new Dictionary<TKey, TVal>();
            foreach (KeyValuePair<TKey, TVal> kvp in source)
            {
                dict.Add(kvp.Key, kvp.Value);
            }
            return dict;
        }
    }
}
