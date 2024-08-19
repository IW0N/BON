using System.Reflection;
using Bon.Enum;
using Bon.Options;

namespace Bon.Converters
{
    public class ObjectConverter : BonConverter<object>
    {
        public override bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsClass || (typeToConvert.IsValueType && !typeToConvert.IsPrimitive);

        public override void Write(BonWriter writer, object data, BonContext context)
        {
            var valueEnumer = new ValueEnumerable(data);
            foreach (var value in valueEnumer)
            {
                var converter = BonSerializer.GetConverter(value.GetType(),context.Options);
                converter.BaseWrite(writer, value, context);
            }
        }

        public override object Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var obj = Activator.CreateInstance(typeToConvert);
            
            var keys = new KeyEnumerable(typeToConvert);
            
            foreach (var key in keys)
            {
                if(key is FieldInfo field)
                {
                    var conv = BonSerializer.GetConverter(field.FieldType);

                    var value = conv.BaseRead(reader, field.FieldType, context);
                    
                    field.SetValue(obj, value);
                }
                else if (key is PropertyInfo prop)
                {
                    var conv = BonSerializer.GetConverter(prop.PropertyType);

                    var value = conv.BaseRead(reader, prop.PropertyType, context);

                    prop.SetValue(obj, value);
                }
            }

            return obj;
        }

    }
}
