using Bon.Options;

namespace Bon.Enum
{
    /// <summary>
    /// Write-only binary write area
    /// </summary>
    public class BinaryWriteArea : BonBinaryEnumerable
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

        public BinaryWriteArea(byte[] data, int count, BonContext context) : base(data, count, context)
        {
        }
    }
}
