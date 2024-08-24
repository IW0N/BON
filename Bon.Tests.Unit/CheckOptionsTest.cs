using Bon.Converters;
using Bon.Options;
using Bon.Serializers;
using Bon.Tests.Unit.Converters;

namespace Bon.Tests.Unit
{
    public class CheckOptionsTest
    {
        [Test]
        public void Test()
        {
            IBonConvertible[] convertibles =
            [
                new TestType2_1Converter(),
                new TestType1_1_1Converter(),
                new TestType1_3_1Converter(),
                new TestType1Converter(),
                new TestType1_1Converter(),
                new TestType2Converter(),
                new TestType3Converter(),
                new TestType2_2Converter(),
                new TestType1_3Converter()
            ];
            
            var convertes = new ConverterList(convertibles);
            var options = new BonOptions { Converters = convertes };


        }
    }
}
