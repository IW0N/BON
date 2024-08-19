using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tson.Options;

namespace Tson.Converters
{
    public interface ITsonConvertible
    {
        public Type Type { get; }

        public bool CanConvert(Type typeToConvert);

        public void Init(TsonOptions options);
    }

    public abstract class TsonConverterFactory : ITsonConvertible
    {
        public abstract Type Type { get; }

        public virtual void Init(TsonOptions options) { }

        public abstract bool CanConvert(Type typeToConvert);

        public abstract TsonConverter BuildConverter(Type outputType, TsonOptions options);
    }
}