using System.Collections.Generic;
using static PageOfBob.Parsing.Compiled.GeneralRules.Rules;
using static PageOfBob.Parsing.Compiled.StringRules.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public static class ExampleCsvParserString
    {
        static IParser<List<string>> parser;

        public static IParser<List<string>> ParseCsvLine() => parser ?? (parser = _ParseCsvLine());

        static IParser<List<string>> _ParseCsvLine()
        {
            var quote = Match('"');
            var comma = Match(',');
            var whitespace = Text(' ', '\t');
            var eol = Text('\r', '\n').Required();

            var doubleQuote = quote.ThenIgnore(quote);
            var quotedContent =
                GetPosition
                .ThenIgnore(Any(quote.Not(), doubleQuote).Many(keepResults: false))
                .ThenCreateString();

            var quotedValue = quote.ThenKeep(quotedContent).ThenIgnore(quote);
            var unquotedValue = Text(x => x != '"' && x != ',' && x != '\n' && x != '\r');
            var value = Any(quotedValue, unquotedValue).ThenIgnore(whitespace);
            var line = value.Many(comma.ThenIgnore(whitespace)).ThenIgnore(eol);

            return line.CompileParser("CsvLineParser");
        }
    }
    
    /*
    public static class ExampleCsvSpanParser
    {
        static IParser<List<StringSpan>> parser;

        public static IParser<List<StringSpan>> ParseCsvLine() => parser ?? (parser = _ParseCsvLine());

        static IParser<List<StringSpan>> _ParseCsvLine()
        {
            var quote = MatchSpan('"');
            var comma = MatchSpan(',');
            var whitespace = MatchSpanMany(x => x == ' ' || x == '\t');
            var eol = MatchSpanMany(x => x == '\r' || x == '\n').Required();

            var doubleQuote = quote.ThenIgnore(quote);
            var quotedContent = Any(MatchSpanMany(x => x != '"').Required(), doubleQuote).ManyAsSpan();

            var quotedValue = quote.ThenKeep(quotedContent).ThenIgnore(quote);
            var unquotedValue = MatchSpanMany(x => x != '"' && x != ',' && x != '\n' && x != '\r');
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

            return line.CompileParser("CsvLineSpanParser");
        }
    }
    */
}
