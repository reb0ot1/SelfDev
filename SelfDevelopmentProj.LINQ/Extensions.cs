namespace SelfDevelopmentProj.LINQ
{
    public static class Extensions
    {
        public static CustomIntEnumerator GetEnumerator(this Range range)
        {
            return new CustomIntEnumerator(range);
        }
    }

    public ref struct CustomIntEnumerator
    {
        private int _current;
        private readonly int _end;

        public CustomIntEnumerator(Range range)
        {
            if (range.End.IsFromEnd)
            {
                throw new NotSupportedException();
            }

            this._current = range.Start.Value - 1;
            this._end = range.End.Value;
        }

        public int Current => _current;

        public bool MoveNext()
        {
            _current++;

            return _current <= _end;
        }
    }
}
