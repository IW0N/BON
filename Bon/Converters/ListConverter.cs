using System.Collections;
using Bon.Options;

namespace Bon.Converters
{
    public class ListConverterFactory : BonConverterFactory
    {
        public override Type Type => typeof(List<>);

        public override BonConverter BuildConverter(Type outputType, BonOptions options)
        {
            var elementType = outputType.GetElementType();
            var conv = Activator.CreateInstance(typeof(ListConverter<>).MakeGenericType(elementType));
            var converter = (BonConverter)conv!;
            converter.Init(options);
            return converter;
        }
    }

    public class ListConverter<T> : EnumerableConverter<List<T>>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == typeof(List<>);
        
        protected override List<T> Cast(IList source, Type valType) => (List<T>)source;
    }
}
