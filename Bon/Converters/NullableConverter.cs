using Bon.Options;

namespace Bon.Converters
{
    
    public class NullableConverterFactory : BonConverterFactory
    {
        public override Type Type => typeof(Nullable<>);

        public override bool CanConvert(Type typeToConvert)=>
            typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == Type;
        
        public override BonConverter BuildConverter(Type outputType, BonOptions options)
        {
            var instance = (BonConverter)Activator.CreateInstance(typeof(NullableConverter<>).MakeGenericType(outputType.GenericTypeArguments));
            instance.Init(options);
            return instance;
        }

    }

    public class NullableConverter<T>:BonConverter<T?> where T:struct
    {
        private BonConverter<T> _valueConverter;
        public override void Init(BonOptions options)
        {
            _valueConverter = BonSerializer.GetConverter<T>(options);
        }

        public override T? Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var isNull = reader.IsNull();
            
            if (isNull)
            {
                return null;
            }

            return _valueConverter.Read(reader, typeToConvert, context);
        }

        public override void Write(BonWriter writer, T? data, BonContext context)
        {
            bool isNull = data is null;
            writer.WriteNullFlag(isNull);
            if (!isNull)
            {
                _valueConverter.Write(writer, data.Value, context);
            }
        }
    }
}
