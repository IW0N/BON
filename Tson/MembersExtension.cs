using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Bon.Attributes;

namespace Bon
{
    public static class MembersExtension
    {
        public static IEnumerable<MemberInfo> GetBonMembers(this Type dataType) => 
            dataType
                .GetMembers()
                .Where(
                    m => (m is PropertyInfo || m is FieldInfo) &&
                    m.GetCustomAttribute<BonIgnoreAttribute>() is null)
                .OrderBy(m => m.Name);
    }
}
