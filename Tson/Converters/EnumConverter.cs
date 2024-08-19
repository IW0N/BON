using System.Reflection;
using Tson.Options;
using static Tson.TsonSerializer;

namespace Tson.Converters
{
    public class EnumConverter : TsonConverter<System.Enum>
    {
        Dictionary<Type, TsonConverter> _convertMap = [];
        public override void Init(TsonOptions options)
        {
            IEnumerable<Type> underTypes = [
                typeof(sbyte), typeof(byte), 
                typeof(short), typeof(ushort), 
                typeof(int), typeof(uint),
                typeof(long), typeof(ulong)
            ];

            foreach (var numT in underTypes)
            {
                var conv = GetConverter(numT, options);
                _convertMap.Add(numT, conv);
            }
        }

        public override System.Enum Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            var underType = System.Enum.GetUnderlyingType(typeToConvert);
            var converter = _convertMap[underType];
            var value = converter.BaseRead(reader, underType, context);

            var enumObj = (System.Enum)Activator.CreateInstance(typeToConvert);
            var valueField = GetEnumValueField(typeToConvert);
            valueField.SetValue(enumObj, value);
            return enumObj;
        }

        public override void Write(TsonWriter writer, System.Enum data, TsonContext context)
        {
            var valueField = GetEnumValueField(data.GetType());
            //4 | 16 | 32 = 00000100 | 00010000 | 00100000 = 00110100v2 = 52v10
            var value = valueField.GetValue(data);

            _convertMap[value.GetType()].BaseWrite(writer, value, context);
        }

        private FieldInfo GetEnumValueField(Type enumType)=> 
            enumType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance).First();
    }
}
