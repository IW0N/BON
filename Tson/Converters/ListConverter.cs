using System.Collections;
using Tson.Options;

namespace Tson.Converters
{
    public class ListConverterFactory : TsonConverterFactory
    {
        public override Type Type => typeof(List<>);

        public override TsonConverter BuildConverter(Type outputType, TsonOptions options)
        {
            var elementType = outputType.GetElementType();
            var conv = Activator.CreateInstance(typeof(ListConverter<>).MakeGenericType(elementType));
            var converter = (TsonConverter)conv!;
            converter.Init(options);
            return converter;
        }

        public override bool CanConvert(Type typeToConvert) => 
            typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(List<>);
    }

    public class ListConverter<T> : EnumerableConverter<List<T>>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(List<>);
        
        protected override List<T> Cast(IList source, Type valType) => (List<T>)source;
    }
}
