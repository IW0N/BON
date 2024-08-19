using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bon.Options;

namespace Bon.Converters
{
    public interface IBonConvertible
    {
        public Type Type { get; }

        public bool CanConvert(Type typeToConvert);

        public void Init(BonOptions options);
    }

    public abstract class BonConverterFactory : IBonConvertible
    {
        public abstract Type Type { get; }

        public virtual void Init(BonOptions options) { }

        public abstract bool CanConvert(Type typeToConvert);

        public abstract BonConverter BuildConverter(Type outputType, BonOptions options);
    }
}