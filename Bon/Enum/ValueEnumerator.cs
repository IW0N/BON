using System.Collections;
using System.Reflection;

namespace Bon.Enum
{
    internal class ValueEnumerator : IEnumerator<KeyValuePair<MemberInfo,object>>
    {
        private readonly object _data;
        private readonly IEnumerator<MemberInfo> _enumerator;

        //if type is ObjectData
        private readonly IEnumerable<MemberInfo> _propsAndFields;

        public KeyValuePair<MemberInfo,object> Current 
        { 
            get 
            {
                var current = _enumerator?.Current;
                if (current is PropertyInfo prop)
                {
                    return new(current, prop.GetValue(_data));
                }
                else if (current is FieldInfo field)
                {
                    return new(current, field.GetValue(_data));
                }
                else
                {
                    throw new Exception("Invalid member type!");
                }
            } 
        }

        object IEnumerator.Current => Current;

        public ValueEnumerator(object data)
        {
            _data = data;
            _propsAndFields = _data.GetType().GetBonMembers();
            _enumerator = _propsAndFields.GetEnumerator();
        }

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset()
        {
            _enumerator.Reset();
        }

        public void Dispose()
        {
            _enumerator.Dispose();
        }
    }
}
