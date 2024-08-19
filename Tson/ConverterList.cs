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

        public ConverterList(IEnumerable<ITsonConvertible> convertibles)
        {
            var sorter = new ConverterListSorter(convertibles);
            _converters = new List<ITsonConvertible>(sorter.Sort());
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
