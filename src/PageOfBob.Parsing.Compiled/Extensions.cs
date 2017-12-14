using System.Collections;
using System.Collections.Generic;

namespace PageOfBob.Parsing.Compiled
{
    public static class Extensions
    {
        public static IEnumerable<T> AsEnumerable<T>(this IParser<T> parser, string input) => new ParserEnumerable<T>(parser, input);
    }

    public struct ParserEnumerable<T> : IEnumerable<T>
    {
        private readonly IParser<T> parser;
        private readonly string input;

        public ParserEnumerable(IParser<T> parser, string input)
        {
            this.parser = parser;
            this.input = input;
        }

        public IEnumerator<T> GetEnumerator() => new ParserEnumerator<T>(parser, input);

        IEnumerator IEnumerable.GetEnumerator() => new ParserEnumerator<T>(parser, input);
    }

    public struct ParserEnumerator<T> : IEnumerator<T>
    {
        private readonly IParser<T> parser;
        private readonly string input;
        int position;

        public ParserEnumerator(IParser<T> parser, string input)
        {
            this.parser = parser;
            this.input = input;
            Current = default(T);
            position = 0;
        }

        public T Current { get; private set; }

        object IEnumerator.Current => Current;

        public void Dispose() { }

        public bool MoveNext()
        {
            bool success = parser.TryParse(input, out T value, out position, position);
            Current = success ? value : default(T);
            return success;
        }

        public void Reset() => position = 0;
    }
}
