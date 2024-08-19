using Bon.Converters;
using Bon.Serializers;
using Bon.Tests.Unit.Converters;

namespace Bon.Tests.Unit
{
    public class TestSorting
    {
        [Test]
        public void Test()
        {
            IBonConvertible[] convertibles = 
            [
                new ListConverterFactory(),
                new IntConverter(),
                new ByteConverter(),
                new TestType2_1Converter(),
                new TestType1_1_1Converter(),
                new ObjectConverter(),
                new TestType1_3_1Converter(),
                new StringConverter(),
                new TestType1Converter(),
                new TestType1_1Converter(),
                new TestType2Converter(),
                new TestType3Converter(),
                new TestType2_2Converter(),
                new TestType1_3Converter()
            ];

            var sorter = new ConverterListSorter(convertibles);
            var sorted = sorter.Sort();

            //после сортировки смотреть вручную, что получилось
            Assert.Pass();
        }
    }
}
