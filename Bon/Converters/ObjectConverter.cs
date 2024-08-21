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
            var isNullable = data.GetType().IsClass;
            if (isNullable)
            {
                var isNull = data is null;
                writer.WriteNullFlag(isNull);
                if (isNull)
                {
                    return;
                }
            }
            
            var valueEnumer = new ValueEnumerable(data);
            
            foreach (var kvp in valueEnumer)
            {
                Type t;
                if(kvp.Key is PropertyInfo p)
                {
                    t = p.PropertyType;
                }
                else if (kvp.Key is FieldInfo f)
                {
                    t = f.FieldType;
                }
                else
                {
                    throw new Exception();
                }
                var converter = BonSerializer.GetConverter(t,context.Options);
                converter.BaseWrite(writer, kvp.Value, context);
            }
        }

        public override object Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            if (typeToConvert.IsClass&&reader.IsNull())
            {
                return null;
            }

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
