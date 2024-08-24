using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bon.Options;

namespace Bon.Converters
{
    /// <summary>
    /// The most basicly part of Bon.<br/>
    /// This interface unite <see cref="BonConverter"/> and <see cref="BonConverterFactory"/>.<br/>
    /// This is should contained in list
    /// </summary>
    public interface IBonConvertible
    {
        /// <summary>
        /// Handlling data type (int, float, MyType, etc.) 
        /// </summary>
        public Type Type { get; }

        /// <summary>
        /// It checks whether to can convert passed type
        /// </summary>
        /// <param name="typeToConvert">Checking type</param>
        /// <returns></returns>
        public bool CanConvert(Type typeToConvert);

        public bool Inited { get; }

        /// <summary>
        /// Invokes after constructing instance.<br/>
        /// Recommend getting any converters or factories here.
        /// </summary>
        /// <param name="options"></param>
        public void Init(BonOptions options);
    }

    /// <summary>
    /// It used for generic converters. Build converter after success invokation <see cref="CanConvert(Type)"/>
    /// </summary>
    public abstract class BonConverterFactory : IBonConvertible
    {
        public abstract Type Type { get; }

        public bool Inited { get; protected set; }

        public virtual void Init(BonOptions options)
        {
            Inited = true;
        }

        public virtual bool CanConvert(Type typeToConvert) =>
            typeToConvert.IsGenericType && typeToConvert.GetGenericTypeDefinition() == Type;

        public abstract BonConverter BuildConverter(Type outputType, BonOptions options);
    }
}