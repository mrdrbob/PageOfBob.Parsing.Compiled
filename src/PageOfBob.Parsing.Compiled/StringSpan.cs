using System.Linq;

namespace PageOfBob.Parsing.Compiled
{
    public struct StringSpan
    {
        public StringSpan(string baseString, int start, int end)
        {
            BaseString = baseString;
            Start = start;
            End = end;
        }

        public string BaseString { get; }
        public int Start { get; }
        public int End { get; }
        public int Length => End - Start;
        public override string ToString() => BaseString.Substring(Start, Length);
    }

    // TODO: ManySpan rule that runs Span rule repeated, tracks the positions, and returns a single
    // span that spans the entire length of the rule.

    public static class StringSpanExtensions
    {
        public static bool Matches(this StringSpan span, string value)
        {
            if (value.Length != span.Length)
                return false;
            return span.BaseString.Skip(span.Start).Take(span.Length).SequenceEqual(value);
        }

        public static StringSpan CombineSequential(this StringSpan span, StringSpan second)
        {
            if (span.BaseString != second.BaseString)
                throw new System.InvalidOperationException();
            if (span.End != second.Start)
                throw new System.InvalidOperationException();
            return new StringSpan(span.BaseString, span.Start, second.End);
        }
    }
}
