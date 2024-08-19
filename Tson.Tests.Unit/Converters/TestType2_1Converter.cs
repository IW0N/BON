using Tson.Converters;
using Tson.Options;
using Tson.Tests.Unit.Types.Sorting;

namespace Tson.Tests.Unit.Converters
{
    internal class TestType2_1Converter : TsonConverter<TestType2_1>
    {
        public override TestType2_1 Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(TsonWriter writer, TestType2_1 data, TsonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
