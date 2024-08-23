using System.Collections;
using System.Reflection;
namespace Bon.Enum
{
    /// <summary>
    /// It is "schema" for deserializable data.<br/>
    /// Contains list of properties and fields. 
    /// Item type is <see cref="PropertyInfo"/> or <see cref="FieldInfo"/>
    /// </summary>
    public class KeyEnumerable : IEnumerable<MemberInfo>
    {
        private readonly IEnumerable<MemberInfo> _propsAndFields;
        public KeyEnumerable(Type rootType)
        {
            _propsAndFields = rootType.GetBonMembers();
        }

        public IEnumerator<MemberInfo> GetEnumerator() => _propsAndFields.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
