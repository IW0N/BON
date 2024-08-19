using System.Collections;
using System.Reflection;
namespace Bon.Enum
{
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
