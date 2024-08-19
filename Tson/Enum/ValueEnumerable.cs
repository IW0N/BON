using System.Collections;

namespace Tson.Enum
{
    public class ValueEnumerable(object data) : IEnumerable
    {
        private readonly object _data = data;

        public IEnumerator GetEnumerator() => new ValueEnumerator(_data);
    }
}
