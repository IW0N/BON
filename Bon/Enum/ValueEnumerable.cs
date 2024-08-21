using System.Collections;
using System.Reflection;

namespace Bon.Enum
{
    public class ValueEnumerable(object data) : IEnumerable<KeyValuePair<MemberInfo,object>>
    {
        private readonly object _data = data;

        public IEnumerator<KeyValuePair<MemberInfo, object>> GetEnumerator()
        {
            return new ValueEnumerator(_data);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new ValueEnumerator(_data);
        }
    }
}
