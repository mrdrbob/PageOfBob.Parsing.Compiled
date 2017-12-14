using Xunit;
using static PageOfBob.Parsing.Compiled.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class StringRulesTests
    {
        [Fact]
        public void MatchMatchesChar()
        {
            var parser = Match('a').CompileParser("MatchMatchesChar");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertFailure("A", 0);
            parser.AssertFailure("b", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void MatchMatchesChars()
        {
            var parser = Match('a', 'b', 'c').CompileParser("MatchMatchesChars");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("b", 'b', 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void MatchInsensitiveMatchesChar()
        {
            var parser = IMatch('a').CompileParser("MatchInsensitiveMatchesChar");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertSuccess("A", 'A', 1);
            parser.AssertFailure("b", 0);
        }

        [Fact]
        public void MatchInsensitiveMatchesChars()
        {
            var parser = IMatch('a', 'b', 'c').CompileParser("MatchInsensitiveMatchesChars");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertSuccess("A", 'A', 1);
            parser.AssertSuccess("b", 'b', 1);
            parser.AssertFailure("d", 0);
        }

        [Fact]
        public void MatchSpanWorks()
        {
            var parser = Match(char.IsLetter).CompileParser("MatchSpanWorks");
            parser.AssertSuccess("abc3", new StringSpan("abc3", 0, 3), 3);
            parser.AssertSuccess("", new StringSpan("", 0, 0), 0);
            parser.AssertSuccess("abc", new StringSpan("abc", 0, 3), 3);

            var result = parser.TryParse("abc3", out StringSpan span, out int position);
            Assert.True(result);
            Assert.True(span.Matches("abc"));
        }

        [Fact]
        public void MatchSpanRequiredWorks()
        {
            var parser = Match(char.IsLetter).Required().CompileParser("MatchSpanRequiredWorks");
            parser.AssertSuccess("abc3", new StringSpan("abc3", 0, 3), 3);
            parser.AssertFailure("3", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void IsLetterWorks()
        {
            var parser = IsLetter.CompileParser("IsLetterWorks");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertSuccess("A", 'A', 1);
            parser.AssertFailure("1", 0);
        }

        [Fact]
        public void IsDigitWorks()
        {
            var parser = IsDigit.CompileParser("IsDigitWorks");
            parser.AssertSuccess("1", '1', 1);
            parser.AssertFailure("a", 0);
        }

        [Fact]
        public void IsLetterOrDigitWorks()
        {
            var parser = IsLetterOrDigit.CompileParser("IsLetterOrDigitWorks");
            parser.AssertSuccess("1", '1', 1);
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertFailure(" ", 0);
        }

        [Fact]
        public void IsWhiteSpaceWorks()
        {
            var parser = IsWhiteSpace.CompileParser("IsWhiteSpaceWorks");
            parser.AssertSuccess(" ", ' ', 1);
            parser.AssertFailure("a", 0);
        }

        [Fact]
        public void ManyAsTextWorks()
        {
            var parser = Match('a', 'b', 'c').ManyAsText().CompileParser("ManyAsTextWorks");
            parser.AssertSuccess("abcdefg", "abc", 3);
            parser.AssertSuccess("defg", "", 0);
            parser.AssertSuccess("", "", 0);
        }

        [Fact]
        public void RequiredTextWorks()
        {
            var parser = Match('a', 'b', 'c').ManyAsText().Required().CompileParser("RequiredTextWorks");
            parser.AssertSuccess("abcdefg", "abc", 3);
            parser.AssertFailure("defg", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void NotNegatesRule()
        {
            var parser = Match('a', 'b', 'c').Not().CompileParser("NotNegatesRule");
            parser.AssertFailure("a", 0);
            parser.AssertFailure("", 0);
            parser.AssertSuccess("d", 'd', 1);
        }

        [Fact]
        public void TextRuleWorks()
        {
            var parser = Text("abc").CompileParser("TextRuleWorks");
            parser.AssertSuccess("abc", "abc", 3);
            parser.AssertSuccess("abcdef", "abc", 3);
            parser.AssertFailure("ab", 0);
            parser.AssertFailure("cbadef", 0);
            parser.AssertFailure("ABCDEF", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void InsensitiveTextRuleWorks()
        {
            var parser = Text("abc", true).CompileParser("InsensitiveTextRuleWorks");
            parser.AssertSuccess("abcdef", "abc", 3);
            parser.AssertFailure("ab", 0);
            parser.AssertFailure("cbadef", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void InsensitiveTextRuleDoesNotKeepOriginalText()
        {
            var parser = Text("abc", true).CompileParser("InsensitiveTextRuleDoesNotKeepOriginalText");
            parser.AssertSuccess("ABCDEF", "abc", 3);
        }
    }
}
