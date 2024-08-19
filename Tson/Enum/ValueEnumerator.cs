using System.Collections;
using System.Reflection;

namespace Tson.Enum
{
    internal class ValueEnumerator : IEnumerator
    {
        private readonly object _data;
        private readonly IEnumerator _enumerator;

        //if type is ObjectData
        private readonly IEnumerable<MemberInfo> _propsAndFields;

        public object? Current 
        { 
            get 
            {
                var current = _enumerator?.Current;
                if (current is PropertyInfo prop)
                {
                    return prop.GetValue(_data);
                }
                else if (current is FieldInfo field)
                {
                    return field.GetValue(_data);
                }
                else
                {
                    throw new Exception("Invalid member type!");
                }
            } 
        }

        public ValueEnumerator(object data)
        {
            _data = data;
            _propsAndFields = _data.GetType().GetTsonMembers();
            _enumerator = _propsAndFields.GetEnumerator();
        }

        public bool MoveNext() => _enumerator.MoveNext();

        public void Reset()
        {
            _enumerator.Reset();
        }
    }
}
