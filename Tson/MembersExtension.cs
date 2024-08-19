﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tson.Attributes;

namespace Tson
{
    public static class MembersExtension
    {
        public static IEnumerable<MemberInfo> GetTsonMembers(this Type dataType) => 
            dataType
                .GetMembers()
                .Where(
                    m => (m is PropertyInfo || m is FieldInfo) &&
                    m.GetCustomAttribute<TsonIgnoreAttribute>() is null)
                .OrderBy(m => m.Name);
    }
}
