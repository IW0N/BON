using System.Collections;
using Bon.Options;
using Bon.Serializers;

namespace Bon.Converters
{
    public class ArrayConverterFactory : BonConverterFactory
    {
        public override Type Type => typeof(Array);

        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.BaseType == typeof(Array);

        public override BonConverter BuildConverter(Type type, BonOptions options)
        {
            var elementType = type.GetElementType();
            var conv = Activator.CreateInstance(typeof(ArrayConverter<>).MakeGenericType(elementType));
            return (BonConverter)conv;
        }
    }

    public class ArrayConverter<T> : EnumerableConverter<T[]>
    {
        protected override T[] Cast(IList source, Type valType)
        {
            var arr = Array.CreateInstance(typeof(T), source.Count);
            for (int i = 0; i < arr.Length; i++)
            {
                arr.SetValue(source[i], i);
            }
            return (T[])arr;
        }
    }
}
