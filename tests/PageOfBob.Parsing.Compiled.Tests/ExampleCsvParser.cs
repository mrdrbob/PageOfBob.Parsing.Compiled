using System.Collections.Generic;
using static PageOfBob.Parsing.Compiled.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public static class ExampleCsvParser
    {
        static IParser<List<string>> parser;

        public static IParser<List<string>> ParseCsvLine() => parser ?? (parser = _ParseCsvLine());

        static IParser<List<string>> _ParseCsvLine()
        {
            var quote = Match('"');
            var comma = Match(',');
            var whitespace = Match(' ', '\t').Many(false);
            var eol = Match('\r', '\n').Many().Required();

            var doubleQuote = quote.ThenIgnore(quote);
            var quotedContent = Any(quote.Not(), doubleQuote).ManyAsText();

            var quotedValue = quote.ThenKeep(quotedContent).ThenIgnore(quote);
            var unquotedValue = Match('"', ',', '\n', '\r').Not().ManyAsText();
            var value = Any(quotedValue, unquotedValue).ThenIgnore(whitespace);
            var secondValue = comma.ThenIgnore(whitespace).ThenKeep(value);

            // TODO: Find a better way to do this.
            // Stupid separators.
            var line = value
                .Then(secondValue.Many(), (first, list) =>
                {
                    list.Insert(0, first);
                    return list;
                })
                .ThenIgnore(eol);

            return line.CompileParser("CsvLineParser");
        }
    }
}
