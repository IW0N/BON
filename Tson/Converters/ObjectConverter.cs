using System.Reflection;
using Tson.Enum;
using Tson.Options;

namespace Tson.Converters
{
    public class ObjectConverter : TsonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsClass || (typeToConvert.IsValueType && !typeToConvert.IsPrimitive);

        public override void Write(TsonWriter writer, object data, TsonContext context)
        {
            var valueEnumer = new ValueEnumerable(data);
            foreach (var value in valueEnumer)
            {
                var converter = TsonSerializer.GetConverter(value.GetType(),context.Options);
                converter.BaseWrite(writer, value, context);
            }
        }

        public override object Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var obj = Activator.CreateInstance(typeToConvert);
            
            var keys = new KeyEnumerable(typeToConvert);
            
            foreach (var key in keys)
            {
                if(key is FieldInfo field)
                {
                    var conv = TsonSerializer.GetConverter(field.FieldType);

                    var value = conv.BaseRead(reader, field.FieldType, context);
                    
                    field.SetValue(obj, value);
                }
                else if (key is PropertyInfo prop)
                {
                    var conv = TsonSerializer.GetConverter(prop.PropertyType);

                    var value = conv.BaseRead(reader, prop.PropertyType, context);

                    prop.SetValue(obj, value);
                }
            }

            return obj;
        }

    }
}
