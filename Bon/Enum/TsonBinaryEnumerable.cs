using System.Collections;
using Bon.Options;

namespace Bon.Enum
{
    public class BonBinaryEnumerable:IEnumerable<byte>
    {
        protected readonly byte[] _data;
        protected readonly int _startIndex;
        protected readonly BonContext _context;
        public readonly int count;

        public BonBinaryEnumerable(byte[] data, int count, BonContext context)
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
