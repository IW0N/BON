using System.Collections;
using System.Reflection;

namespace Bon.Enum
{
    /// <summary>
    /// It converts a custom class or struct to list of <see cref="KeyValuePair{MemberInfo, object}"/><br/> 
    /// where key is <see cref="FieldInfo"/> or <see cref="PropertyInfo"/><br/>
    /// value is key's value in passed data
    /// </summary>
    /// <param name="data"></param>
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
