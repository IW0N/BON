using Bon.Options;

namespace Bon.Enum
{
    public class BinaryReadArea : BonBinaryEnumerable
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


        public BinaryReadArea(byte[] data, int count, BonContext context) : base(data, count, context)
        {
            
        }

        
    }
}
