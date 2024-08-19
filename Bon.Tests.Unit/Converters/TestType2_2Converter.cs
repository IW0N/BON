using Bon.Converters;
using Bon.Options;
using Bon.Tests.Unit.Types.Sorting;

namespace Bon.Tests.Unit.Converters
{
    internal class TestType2_2Converter : BonConverter<TestType2_2>
    {
        public override TestType2_2 Read(BonReader reader, Type typeToConvert, BonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(BonWriter writer, TestType2_2 data, BonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
