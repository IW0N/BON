using Tson.Converters;
using Tson.Options;
using Tson.Tests.Unit.Types.Sorting;

namespace Tson.Tests.Unit.Converters
{
    internal class TestType3Converter : TsonConverter<TestType3>
    {
        public override TestType3 Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(TsonWriter writer, TestType3 data, TsonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
