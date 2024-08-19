using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bon.Options;

namespace Bon.Converters
{
    /*
    ---Концепция---

    Любое передаваемое значение есть массив значений с определённой длиной.
    Кодировка для строк по умолчанию utf8.
    Есть 2 типа значений: 
    1. Значения с фиксированной длиной
    2. Массивы и объекты
    
    1 - тип будет конвертироваться в такой формат [данные].
      Размер определяется в зависимости от типа декодируемой переменной
    2 - в такой [длина (n байт), данные].
      Размер длины массива/объекта указывается в настройках сериализатора.
      По умолчанию - 2 байта


    --Пример--
    abc = { int c = 23, byte a = 62, string d = "hello, string", obj b = { int a = 17, long r = 610, ushort l = 256 } }
    --> [0,0,0,23, 62, 0,13,104,101,108,108,111,044,032,115,116,114,105,110,103, 0,14,0,0,0,17, 0,0,0,0,0,0,2,98, 1,0]
     длина = 4+1+2+13+2+14 = 5+16+16 = 37 байт
    */
    public abstract class BonConverter : IBonConvertible
    {
        public virtual Type Type { get; }

        public virtual bool CanConvert(Type typeToConvert) =>
            typeToConvert == Type;

        public virtual void Init(BonOptions options) { }

        public abstract void BaseWrite(BonWriter writer, object data, BonContext context);

        public abstract object BaseRead(BonReader reader, Type typeToConvert, BonContext context);
    }

    public abstract class BonConverter<T> : BonConverter
    {
        public override Type Type => typeof(T);

        public abstract void Write(BonWriter writer, T data, BonContext context);

        public abstract T Read(BonReader reader, Type typeToConvert, BonContext context);

        public override void BaseWrite(BonWriter writer, object data, BonContext context)
        {
            Write(writer, (T)data, context);
        }

        public override object BaseRead(BonReader reader, Type typeToConvert, BonContext context)
        {
            return Read(reader, typeToConvert, context);
        }
    }
}
