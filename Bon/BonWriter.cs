using System;
using System.Drawing;
using Bon.Converters;
using Bon.Options;

namespace Bon
{
    public class BonWriter
    {
        private readonly IList<byte> _data;
        private readonly BonContext _context;
        private readonly BonConverter _lengthConverter;
        private bool _rewriteDataLength;
        private readonly EditableStack<int> _dataArrayLengths;
        private readonly Stack<int> _dataArrayStarts;
        private readonly LengthSize _lenSize;
        private bool InsideDataArray => _dataArrayLengths.Count > 0;
        private Type LengthType => _lenSize.GetSizeType();

        public BonWriter(IList<byte> data, BonContext context)
        {
            _data = data;
            _context = context;
            _lenSize = _context.Options.DataArrayLengthSize;
            _lengthConverter = BonSerializer.GetConverter(_lenSize.GetSizeType(), _context.Options);
            _dataArrayStarts = new Stack<int>();
            _dataArrayLengths = new EditableStack<int>();
            _rewriteDataLength = false;
        }
        
        public void BeginWriteDataArray()
        {
            var size = (int)_lenSize;
            
            for(int i=0;i<size;i++)
            {
                _data.Add(0);
            }

            RewriteDataLengthsIfRequired(_context.Index, size, RewriteMode.UseDelta);
            
            _dataArrayStarts.Push(_context.Index);
            _dataArrayLengths.Push(0);
            
            _context.Index += size;
        }

        public void WriteData<T>(T data)
        {
            var converter = BonSerializer.GetConverter<T>(_context.Options) ?? throw new Exception("");
            var prevEnd = _context.Index;
            converter.Write(this, data, _context);
            var newEnd = _context.Index;

            RewriteDataLengthsIfRequired(prevEnd, newEnd, RewriteMode.UseNewEndPos);
        }

        public void WriteData(object data, BonConverter converter)
        {
            var prevEndPos = _context.Index;
            converter.BaseWrite(this, data, _context);
            var endPosition = _context.Index;

            RewriteDataLengthsIfRequired(prevEndPos, endPosition, RewriteMode.UseNewEndPos);
        }

        public void WriteBytes(byte[] data)
        {
            if (_rewriteDataLength)
            {
                for (int index=0;index<data.Length;index++)
                {
                    _data[_context.Index + index] = data[index];
                }
            }
            else
            {
                int prevEndPos = _context.Index;
                foreach (var b in data)
                {
                    _data.Add(b);
                }

                RewriteDataLengthsIfRequired(prevEndPos, data.Length, RewriteMode.UseDelta);
            }

            _context.Index += data.Length;
        }

        public void WriteByte(byte b)
        {
            if (_rewriteDataLength) 
            {
                _data[_context.Index] = b;
            }
            else
            {
                int prev = _context.Index;
                _data.Add(b);
                
                RewriteDataLengthsIfRequired(prev, 1, RewriteMode.UseDelta);
                
            }
            _context.Index++;
        }

        public void EndWriteDataArray()
        {
            _dataArrayStarts.Pop();
            _dataArrayLengths.Pop();
        }

        public byte[] ToByteArray() => _data.ToArray();

        private void RewriteDataLengthsIfRequired(int prevEndPos, int b, RewriteMode mode)
        {
            if (!InsideDataArray)
            {
                return;
            }

            _rewriteDataLength = true;
            var delta = mode == RewriteMode.UseNewEndPos ? b - prevEndPos : b;
            int index = 0;
            for (; index < _dataArrayLengths.Count; index++)
            {
                _dataArrayLengths[index] += delta;
            }
            index = 0;

            foreach (var start in _dataArrayStarts)
            {
                var len = _dataArrayLengths[index++];
                _context.Index = start;
                var len1 = Convert.ChangeType(len, LengthType);
                _lengthConverter.BaseWrite(this, len1, _context);
            }
            
            if (mode == RewriteMode.UseNewEndPos)
            {
                _context.Index = b;
            }
            else
            {
                _context.Index = prevEndPos;
            }
            _rewriteDataLength = false;
        }

        
    }
    enum RewriteMode
    {
        UseDelta,
        UseNewEndPos
    }
}
