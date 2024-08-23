using System;
using System.Collections;
using Bon.Converters;
using Bon.Options;

namespace Bon
{
    /// <summary>
    /// List of <see cref="IBonConvertible"/> that contains items as tree struct
    /// </summary>
    public class ConverterList : IEnumerable<IBonConvertible>
    {
        private IList<Type> _valueTypes = [];
        private IList<IBonConvertible> _converters = [];

        public ConverterList(IEnumerable<IBonConvertible> convertibles)
        {
            var sorter = new ConverterListSorter(convertibles);
            _converters = new List<IBonConvertible>(sorter.Sort());
        }

        public void Add(IBonConvertible convertible)
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

        public void AddRange(IEnumerable<IBonConvertible> convertibles)
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

        public IEnumerator<IBonConvertible> GetEnumerator() => _converters.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
