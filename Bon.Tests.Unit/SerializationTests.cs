using Bon.Tests.Unit.Types.Serialization;
using System.Text.Json;
using System.Text;

namespace Bon.Tests.Unit
{
    enum Weeks
    {
        Monday, 
        Tuesday,
        Wednesday,
        Thursday,
        Friday,
        Saturday,
        Sunday
    }

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
                //b = 23.57f,
                b = null,
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
            var deser = BonSerializer.Deserialize<Serialize1>(bytes);

            var arraysEqual = EnumerablesAreEqual(src.array, deser.array);
            var dictsEqual = EnumerablesAreEqual(src.dict, deser.dict);

            var equals = src.String == deser.String &&
                src.a == deser.a &&
                src.c == deser.c &&
                src.b == deser.b &&
                src.test.val == deser.test.val &&
                src.test.www == deser.test.www &&
                arraysEqual &&
                src.abra.val == deser.abra.val &&
                src.abra.www == deser.abra.www &&
                dictsEqual;

            Assert.That(equals);
        }

        [Test]
        public void TestEnum()
        {
            var testVal = Weeks.Wednesday;
            var serialized = BonSerializer.Serialize(testVal);
            var deserVal = BonSerializer.Deserialize<Weeks>(serialized);
            Assert.That(deserVal == testVal);
        }
        
        private bool EnumerablesAreEqual<T>(IEnumerable<T> a, IEnumerable<T> b)
        {
            if(a.Count() != b.Count())
            {
                return false;
            }

            var bEnumer = b.GetEnumerator();
            foreach (var aVal in a)
            {
                bEnumer.MoveNext();
                var bVal = bEnumer.Current;
                if (aVal is null && bVal is not null)
                {
                    return false;
                }
                if (aVal is not null && !aVal.Equals(bVal))
                {
                    return false;
                }
            }

            return true;
        }
    }
}