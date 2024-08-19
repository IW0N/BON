using Tson.Tests.Unit.Types;
using Tson;
using Tson.Tests.Unit.Types.Serialization;
using Tson.Enum;
using System.Text.Json;

namespace Tson.Tests.Unit
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
                array = ["hey", "bro", "how are you?"],
                test = new Serialize2
                {
                    val = 3.14,
                    www = 404
                }
            };
            
            var bytes = TsonSerializer.Serialize(src);
            bytes = TsonSerializer.Serialize(src);
            var deser = TsonSerializer.Deserialize<Serialize1>(bytes);

            //todo: дописать проверку на валидность десериализованных значений
            Assert.Pass();
        }
    }
}