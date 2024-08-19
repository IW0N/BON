using System.Collections;
using Tson.Options;

namespace Tson.Enum
{
    public class TsonBinaryEnumerable:IEnumerable<byte>
    {
        protected readonly byte[] _data;
        protected readonly int _startIndex;
        protected readonly TsonContext _context;
        public readonly int count;

        public TsonBinaryEnumerable(byte[] data, int count, TsonContext context)
        {
            _data = data;
            this.count = count;
            _startIndex = context.Index;
            _context = context;
        }

        public IEnumerator<byte> GetEnumerator()
        {
            for(int x = 0; x < count; x++)
            {
                yield return _data[_startIndex + x];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
