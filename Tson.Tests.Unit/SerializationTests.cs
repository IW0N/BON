using Bon.Tests.Unit.Types;
using Bon;
using Bon.Tests.Unit.Types.Serialization;
using Bon.Enum;
using System.Text.Json;

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
                array = ["hey", "bro", "how are you?"],
                test = new Serialize2
                {
                    val = 3.14,
                    www = 404
                }
            };
            
            var bytes = BonSerializer.Serialize(src);
            bytes = BonSerializer.Serialize(src);
            var deser = BonSerializer.Deserialize<Serialize1>(bytes);

            //todo: дописать проверку на валидность десериализованных значений
            Assert.Pass();
        }
    }
}