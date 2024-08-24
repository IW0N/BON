using Bon.Converters;

namespace Bon.Options
{
    /// <summary>
    /// Contains options of serialization context
    /// </summary>
    public class BonOptions
    {
        public LengthSize DataArrayLengthSize { get; set; } = LengthSize.UInt16;

        private ConverterList _converters=new ConverterList([]);
        public ConverterList Converters
        { 
            get => _converters;
            set 
            {
                _converters = value;

                BonSerializer.Init();
                _converters.AddRange(BonSerializer.RootOptions.Converters);
                foreach (var val in value)
                {
                    if (!val.Inited)
                    {
                        val.Init(this);
                    }
                }
            }
        }

        public BonOptions() { }

        internal BonOptions(LengthSize lenSize, ConverterList list)
        {
            _converters = list;
            DataArrayLengthSize = lenSize;
        }
    }
}