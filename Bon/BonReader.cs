using System.ComponentModel.DataAnnotations;
using Bon.Converters;
using Bon.Enum;
using Bon.Options;

namespace Bon
{
    /// <summary>
    /// Provides base methods for reading data from byte array
    /// </summary>
    public class BonReader
    {
        private readonly byte[] _data;
        private readonly BonContext _context;

        public BonReader(byte[] data, BonContext context)
        {       
            _data = data;
            _context = context;
        }

        public bool IsNull()
        {
            ReadByte(out var flag);
            return flag == 0;
        }

        public int ReadDataLength()
        {
            var start = _context.Index;
            var lenSizeType = _context.Options.DataArrayLengthSize;
            var length = lenSizeType switch
            {
                LengthSize.Byte => _data[start],
                LengthSize.UInt16 => BitConverter.ToUInt16(_data, start),
                LengthSize.Int32 => BitConverter.ToInt32(_data, start),
                _ => throw new Exception("Unknow length size!")
            };

            _context.Index += (int)lenSizeType;
            return length;
        }

        public BinaryReadArea ReadBytes(int count)
        {
            if (count + _context.Index > _data.Length)
            {
                throw new OverflowException();
            }
            var area = new BinaryReadArea(_data,count,_context);
            _context.Index += count;
            return area;
        }

        public bool ReadByte(out byte data)
        {
            if (_context.Index >= _data.Length)
            {
                data = 0;
                return false;
            }
            data = _data[_context.Index++];
            return true;
        }

        /// <summary>
        /// Read bytes and parse them to typed value
        /// <para>
        /// <c>ATTENTION!</c> This may lead to infity cycle 
        /// if it was invoked inside <see cref="BonConverter{T}"/> and <c>T</c> is <typeparamref name="T"/>
        /// </para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T ReadData<T>()
        {
            var converter = BonSerializer.GetConverter<T>(_context.Options);
            return converter.Read(this, typeof(T), _context);
        }

        public BinaryReadArea ReadDataArrayBytes()
        {
            var length = ReadDataLength();
            return new BinaryReadArea(_data, length, _context);
        }
    }
}
