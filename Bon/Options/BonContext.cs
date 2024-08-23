namespace Bon.Options
{
    /// <summary>
    /// Context of serialization.<br/>
    /// Contains current index of bytes array for <see cref="BonReader"/> and <see cref="BonWriter"/><br/>
    /// And <see cref="Options"/> for current serialization context
    /// </summary>
    public class BonContext
    {
        public int Index { get; set; }

        public BonOptions Options { get; init; }
    }
}
