using System.Collections;
using Bon.Options;

namespace Bon.Converters
{
    public abstract class EnumerableConverter<T> : BonConverter<T> where T:IEnumerable 
    {
        private readonly Type _enumerableType = typeof(IEnumerable);

        public override T Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            if (typeToConvert.IsClass && reader.IsNull())
            {
                return default;
            }

            var valueType = GetValueType(typeToConvert);
            var converter = BonSerializer.GetConverter(valueType);
            var listType = typeof(List<>).MakeGenericType(valueType);
            var list = (IList)Activator.CreateInstance(listType);
            var end = reader.ReadDataLength()+context.Index;

            while (context.Index < end)
            {
                var payload = converter.BaseRead(reader, valueType, context);
                list.Add(payload);
            }
            
            return Cast(list, valueType);
        }

        protected abstract T Cast(IList source, Type valType);

        public override void Write(BonWriter writer, T data, BonContext context)
        {
            if (typeof(T).IsClass)
            {
                writer.WriteNullFlag(data is null);
            }

            var valueType = GetValueType(data.GetType());
            var converter = BonSerializer.GetConverter(valueType);
            writer.BeginWriteDataArray();
            foreach (var value in data)
            {
                converter.BaseWrite(writer, value, context);
            }
            writer.EndWriteDataArray();
            //end write 
        }

        private Type GetValueType(Type listType)
        {
            var interfaces = listType.GetInterfaces();
            var baseIenum = typeof(IEnumerable<>);

            listType = interfaces.First(i => 
                i.IsGenericType && i.GetGenericTypeDefinition() == baseIenum);
            
            return listType.GenericTypeArguments[0];
        }
    }
}
