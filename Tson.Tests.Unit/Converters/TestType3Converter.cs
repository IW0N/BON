using Bon.Converters;
using Bon.Options;
using Bon.Tests.Unit.Types.Sorting;

namespace Bon.Tests.Unit.Converters
{
    internal class TestType3Converter : BonConverter<TestType3>
    {
        public override TestType3 Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(BonWriter writer, TestType3 data, BonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
