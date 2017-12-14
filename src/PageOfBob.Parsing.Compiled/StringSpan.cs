using System.Linq;

namespace PageOfBob.Parsing.Compiled
{
    public struct StringSpan
    {
        private readonly string str;

        public StringSpan(string str, int start, int end)
        {
            this.str = str;
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
        public int Length => End - Start;
        public override string ToString() => str.Substring(Start, Length);
        public bool Matches(string value)
        {
            if (value.Length != Length)
                return false;
            return str.Skip(Start).Take(Length).SequenceEqual(value);
        }
    }
}
