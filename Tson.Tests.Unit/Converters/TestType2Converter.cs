using Tson.Converters;
using Tson.Options;
using Tson.Tests.Unit.Types.Sorting;

namespace Tson.Tests.Unit.Converters
{
    internal class TestType2Converter : TsonConverter<TestType2>
    {
        public override TestType2 Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(TsonWriter writer, TestType2 data, TsonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
