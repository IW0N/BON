using System;
using System.Collections;
using Tson.Converters;
using Tson.Options;

namespace Tson
{
    public class ConverterList : IEnumerable<ITsonConvertible>
    {
        private IList<Type> _valueTypes = [];
        private IList<ITsonConvertible> _converters = [];
        private readonly TsonOptions _options;

        public ConverterList(TsonOptions options)
        {
            _options = options;
        }
        public void Add(ITsonConvertible convertible)
        {
            var elementType = convertible.Type;
            
            var index = GetIndex(elementType);
            if (index == -1)
            {
                _valueTypes.Add(elementType);
                _converters.Add(convertible);
                return;
            }

            _valueTypes.Insert(index, elementType);
            _converters.Insert(index, convertible);
        }

        public void AddRange(IEnumerable<ITsonConvertible> convertibles)
        {
            foreach (var item in convertibles)
            {
                Add(item);
            }
        }
        public TsonConverter<T> GetConverter<T>()
        {
            var findingType = typeof(T);
            var convertible = _converters.First(c=>c.CanConvert(findingType));
            if(convertible is TsonConverter<T> converter)
            {
                return converter;
            }
            var factory = (TsonConverterFactory)convertible;
            return (TsonConverter<T>)factory.BuildConverter(findingType,_options);
        }
        
        private int GetIndex(Type elementType)
        {
            var lastIndex = _valueTypes.Count - 1;
            var started = false;

            for (var index = 0; index <= lastIndex; index++)
            {
                var vt = _valueTypes[index];
                var assigned = elementType.IsAssignableTo(vt);
                if (!started && assigned)
                {
                    started = true;
                }
                else if (started && !assigned)
                {
                    return index;
                }
            }

            return -1;
        }

        public IEnumerator<ITsonConvertible> GetEnumerator() => _converters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
