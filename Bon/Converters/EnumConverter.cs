using System.Reflection;
using Bon.Options;
using static Bon.BonSerializer;

namespace Bon.Converters
{
    public class EnumConverter : BonConverter<System.Enum>
    {
        Dictionary<Type, BonConverter> _convertMap = [];
        public override void Init(BonOptions options)
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

            Inited = true;
        }

        public override System.Enum Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            var underType = System.Enum.GetUnderlyingType(typeToConvert);
            var converter = _convertMap[underType];
            var value = converter.BaseRead(reader, underType, context);

            var enumObj = (System.Enum)Activator.CreateInstance(typeToConvert);
            var valueField = GetEnumValueField(typeToConvert);
            valueField.SetValue(enumObj, value);
            return enumObj;
        }

        public override void Write(BonWriter writer, System.Enum data, BonContext context)
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
