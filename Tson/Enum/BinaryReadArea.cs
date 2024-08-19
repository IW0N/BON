using Tson.Options;

namespace Tson.Enum
{
    public class BinaryReadArea : TsonBinaryEnumerable
    {
        public byte this[int localIndex]
        {
            get
            {
                var index = _startIndex+localIndex;
                if (index >= count)
                {
                    throw new OverflowException();
                }
                return _data[index];
            }
        }


        public BinaryReadArea(byte[] data, int count, TsonContext context) : base(data, count, context)
        {
            
        }

        
    }
}
