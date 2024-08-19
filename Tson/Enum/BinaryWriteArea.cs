using Tson.Options;

namespace Tson.Enum
{
    public class BinaryWriteArea : TsonBinaryEnumerable
    {
        public byte this[int localIndex] { 
            set 
            {
                var index = _startIndex+localIndex;
                if (localIndex >= count)
                {
                    throw new OverflowException();
                }

                _context.Index = index;
                _data[index] = value;
            } 
        }

        public BinaryWriteArea(byte[] data, int count, TsonContext context) : base(data, count, context)
        {
        }
    }
}
