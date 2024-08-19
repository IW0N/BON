using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tson.Options;

namespace Tson.Converters
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
    public abstract class TsonConverter : ITsonConvertible
    {
        public virtual Type Type { get; }

        public virtual bool CanConvert(Type typeToConvert) =>
            typeToConvert == Type;

        public virtual void Init(TsonOptions options) { }

        public abstract void BaseWrite(TsonWriter writer, object data, TsonContext context);

        public abstract object BaseRead(TsonReader reader, Type typeToConvert, TsonContext context);
    }

    public abstract class TsonConverter<T> : TsonConverter
    {
        public override Type Type => typeof(T);

        public abstract void Write(TsonWriter writer, T data, TsonContext context);

        public abstract T Read(TsonReader reader, Type typeToConvert, TsonContext context);

        public override void BaseWrite(TsonWriter writer, object data, TsonContext context)
        {
            Write(writer, (T)data, context);
        }

        public override object BaseRead(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            return Read(reader, typeToConvert, context);
        }
    }
}
