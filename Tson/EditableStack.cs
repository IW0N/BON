namespace Tson
{
    internal class EditableStack<T> : List<T>
    {
        private int BaseLast => Count - 1;

        public new T this[int index]
        {
            get => base[BaseLast - index];
            set => base[BaseLast - index] = value;
        }

        public new IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < Count; i++)
            {
                yield return this[i];
            }
        }

        public void Push(T value) => Add(value);

        public T Peek() => this.Last();

        public T Pop()
        {
            var val = this.Last();
            RemoveAt(BaseLast);
            return val;
        }
    }
}
