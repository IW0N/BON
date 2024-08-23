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
        /// Check it whether to can converts passed type
        /// </summary>
        /// <param name="typeToConvert">Checking type</param>
        /// <returns></returns>
        public bool CanConvert(Type typeToConvert);

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

        public virtual void Init(BonOptions options) { }

        public abstract bool CanConvert(Type typeToConvert);

        public abstract BonConverter BuildConverter(Type outputType, BonOptions options);
    }
}