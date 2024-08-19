using Tson.Converters;
using Tson.Options;
using Tson.Tests.Unit.Types.Sorting;

namespace Tson.Tests.Unit.Converters
{
    internal class TestType2_2Converter : TsonConverter<TestType2_2>
    {
        public override TestType2_2 Read(TsonReader reader, Type typeToConvert, TsonContext context)
        {
            throw new NotImplementedException();
        }

        public override void Write(TsonWriter writer, TestType2_2 data, TsonContext context)
        {
            throw new NotImplementedException();
        }
    }
}
