namespace Bon.Options
{
    /// <summary>
    /// Size of array length in bytes
    /// </summary>
    public enum LengthSize : byte
    {
        Byte = 1,
        UInt16 = 2,
        Int32 = 4
    }

    public static class LengthSizeExtension
    {
        private static Dictionary<LengthSize, Type> _map = new()
        {
            { LengthSize.Byte, typeof(byte) },
            { LengthSize.UInt16, typeof(ushort) },
            { LengthSize.Int32, typeof(int) }
        };
        /// <summary>
        /// It gets <see cref="Type"/> of varible by <see cref="LengthSize"/>
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public static Type GetSizeType(this LengthSize size) => _map[size];
    }
}
