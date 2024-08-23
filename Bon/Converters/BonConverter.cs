using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Bon.Options;

namespace Bon.Converters
{
    /// <summary>
    /// Base converter for type
    /// </summary>
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
