using Bon.Tests.Unit.Types;
using Bon;
using Bon.Tests.Unit.Types.Serialization;
using Bon.Enum;
using System.Text.Json;
using System.Reflection;
using System.Text;

namespace Bon.Tests.Unit
{
    public class SerializationTests
    {
        [Test]
        public void Test1()
        {
            var src = new Serialize1
            {
                String = "’улЄу, ворд",
                a = 14,
                c = 67,
                b = 23.57f,
                test = new Serialize2
                {
                    val = 3.14,
                    www = 404
                },
                array = ["hey", "bro", "how are you?"],
                abra = new Serialize2
                {
                    val = 999,
                    www = 127001
                },
                dict = new()
                {
                    { 22,"22" },
                    { 100,"100" }
                }
            };

            var bytes = BonSerializer.Serialize(src);
            var bJson = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(src)); 
            var deser = BonSerializer.Deserialize<Serialize1>(bytes);
            var equals = Compare(src, deser);

            Assert.That(equals);
        }

        private bool Compare(object obj1, object obj2)
        {
            if (obj1 == null || obj2 == null)
            {
                return false;
            }
            if (!obj1.GetType().Equals(obj2.GetType()))
            {
                return false;
            }

            Type type = obj1.GetType();
            if (type.IsPrimitive || typeof(string).Equals(type))
            {
                return obj1.Equals(obj2);
            }
            if (type.IsArray)
            {
                Array first = obj1 as Array;
                Array second = obj2 as Array;
                var en = first.GetEnumerator();
                int i = 0;
                while (en.MoveNext())
                {
                    if (!Compare(en.Current, second.GetValue(i)))
                        return false;
                    i++;
                }
            }
            else
            {
                foreach (PropertyInfo pi in type.GetProperties(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    var val = pi.GetValue(obj1);
                    var tval = pi.GetValue(obj2);
                    if (!Compare(val, tval))
                        return false;
                }
                foreach (FieldInfo fi in type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Public))
                {
                    var val = fi.GetValue(obj1);
                    var tval = fi.GetValue(obj2);
                    if (!Compare(val, tval))
                        return false;
                }
            }
            return true;
        }

        private bool IsNullable(Type type)
        {
            if (type.IsClass)
            {
                return true;
            }

            while (type != null)
            {

            }

            return false;
        }
    }
}