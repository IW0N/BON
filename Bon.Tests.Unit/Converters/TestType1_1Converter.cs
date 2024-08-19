using Bon.Converters;
using Bon.Options;
using Bon.Tests.Unit.Types.Sorting;

namespace Bon.Tests.Unit.Converters
{
    internal class TestType1_1Converter : BonConverter<TestType1_1>
    {
        public override TestType1_1 Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(BonWriter writer, TestType1_1 data, BonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
