using Bon.Options;

namespace Bon.Enum
{
    /// <summary>
    /// Area of reading bytes<br/>
    /// It needs to not create new byte array
    /// </summary>
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
