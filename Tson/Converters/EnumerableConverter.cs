using System.Collections;
using Tson.Options;
using System.Reflection;

namespace Tson.Converters
{
    public abstract class EnumerableConverter<T> : TsonConverter<T> where T:IEnumerable 
    {
        private readonly Type _enumerableType = typeof(IEnumerable);

        public override T Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var valueType = GetValueType(typeToConvert);
            var converter = TsonSerializer.GetConverter(valueType);
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

        public override void Write(TsonWriter writer, T data, TsonContext context)
        {
            var valueType = GetValueType(data.GetType());
            var converter = TsonSerializer.GetConverter(valueType);
            var fist = context.Index;
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
