using Tson.Converters;
using Tson.Options;
using Tson.Tests.Unit.Types.Sorting;

namespace Tson.Tests.Unit.Converters
{
    internal class TestType1Converter : TsonConverter<TestType1>
    {
        public override TestType1 Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(TsonWriter writer, TestType1 data, TsonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
