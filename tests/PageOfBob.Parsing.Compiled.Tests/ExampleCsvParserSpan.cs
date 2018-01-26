using System.Collections.Generic;
using System.Linq;
using static PageOfBob.Parsing.Compiled.GeneralRules.Rules;
using static PageOfBob.Parsing.Compiled.SpanRules.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public static class ExampleCsvParserSpan
    {
        static IParser<List<StringSpan>> parser;

        public static IParser<List<StringSpan>> ParseCsvLine() => parser ?? (parser = _ParseCsvLine());

        static IParser<List<StringSpan>> _ParseCsvLine()
        {
            var quote = Match('"');
            var comma = Match(',');
            var whitespace = Text(' ', '\t');
            var eol = Text('\r', '\n').Required();

            var doubleQuote = quote.ThenIgnore(quote);
            var quotedContent = GetPosition
                .ThenIgnore(Any(quote.Not(), doubleQuote).Many(keepResults: false))
                .ThenCreateSpan();

            var quotedValue = quote.ThenKeep(quotedContent).ThenIgnore(quote);
            var unquotedValue = Text(x => x != '"' && x != ',' && x != '\n' && x != '\r');
            var value = Any(quotedValue, unquotedValue).ThenIgnore(whitespace);
            var line = value.Many(comma.ThenIgnore(whitespace)).ThenIgnore(eol);

            return line.CompileParser("CsvLineParserSpan");
        }
    }
}
