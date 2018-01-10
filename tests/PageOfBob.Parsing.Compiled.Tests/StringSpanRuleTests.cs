using Xunit;
using static PageOfBob.Parsing.Compiled.SpanRules.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class StringSpanRuleTests
    {
        [Fact]
        public void MatchMatchesChars()
        {
            var parser = Match('a', 'b', 'c').CompileParser("MatchMatchesChars");
            parser.AssertSuccess("a", new StringSpan("a", 0, 1), 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("b", new StringSpan("b", 0, 1), 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void IMatchMatchesChars()
        {
            var parser = IMatch('a', 'b', 'c').CompileParser("IMatchMatchesChars");
            parser.AssertSuccess("a", new StringSpan("a", 0, 1), 1);
            parser.AssertSuccess("A", new StringSpan("A", 0, 1), 1);
            parser.AssertSuccess("b", new StringSpan("b", 0, 1), 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void IsDigitWorks()
        {
            var parser = IsDigit.CompileParser("IsDigitWorks");
            parser.AssertSuccess("0", new StringSpan("0", 0, 1), 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("9", new StringSpan("9", 0, 1), 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void MatchFuncWorks()
        {
            var parser = Match(char.IsDigit).CompileParser("MatchFuncWorks");
            parser.AssertSuccess("0", new StringSpan("0", 0, 1), 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("9", new StringSpan("9", 0, 1), 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void NotWorks()
        {
            var parser = IsDigit.Not().CompileParser("NotWorks");
            parser.AssertFailure("0", 0);
            parser.AssertFailure("", 0);
            parser.AssertSuccess("a", new StringSpan("a", 0, 1), 1);
        }

        [Fact]
        public void MatchMatchesCharsText()
        {
            var parser = Text('a', 'b', 'c').CompileParser("MatchMatchesCharsText");
            parser.AssertSuccess("a", new StringSpan("a", 0, 1), 1);
            parser.AssertSuccess("A", new StringSpan("A", 0, 0), 0);
            parser.AssertSuccess("bca3", new StringSpan("bca3", 0, 3), 3);
        }

        [Fact]
        public void IMatchMatchesCharsText()
        {
            var parser = IText('a', 'b', 'c').CompileParser("IMatchMatchesCharsText");
            parser.AssertSuccess("a", new StringSpan("a", 0 , 1), 1);
            parser.AssertSuccess("A", new StringSpan("A", 0, 1), 1);
            parser.AssertSuccess("Z", new StringSpan("Z", 0, 0), 0);
            parser.AssertSuccess("bca3", new StringSpan("bca3", 0, 3), 3);
        }

        [Fact]
        public void IsDigitWorksText()
        {
            var parser = IsDigitText.CompileParser("IsDigitWorksText");
            parser.AssertSuccess("000", new StringSpan("000", 0, 3), 3);
            parser.AssertSuccess("A", new StringSpan("A", 0, 0), 0);
            parser.AssertSuccess("909a", new StringSpan("909a", 0, 3), 3);
        }

        [Fact]
        public void TextFuncWorks()
        {
            var parser = Text(char.IsDigit).CompileParser("TextFuncWorks");
            parser.AssertSuccess("000", new StringSpan("000", 0, 3), 3);
            parser.AssertSuccess("A", new StringSpan("A", 0, 0), 0);
            parser.AssertSuccess("909a", new StringSpan("909a", 0, 3), 3);
        }

        [Fact]
        public void TextWorks()
        {
            var parser = Text("abc").CompileParser("TextWorks");
            parser.AssertFailure("", 0);
            parser.AssertFailure("ab", 0);
            parser.AssertFailure("ABC", 0);
            parser.AssertSuccess("abc", new StringSpan("abc", 0, 3), 3);
            parser.AssertSuccess("abcefg", new StringSpan("abcefg", 0, 3), 3);
        }

        [Fact]
        public void ITextWorks()
        {
            var parser = IText("abc").CompileParser("ITextWorks");
            parser.AssertFailure("", 0);
            parser.AssertFailure("ab", 0);
            parser.AssertSuccess("ABC", new StringSpan("ABC", 0, 3), 3);
            parser.AssertSuccess("abc", new StringSpan("abc", 0, 3), 3);
            parser.AssertSuccess("abcefg", new StringSpan("abcefg", 0, 3), 3);
        }

        [Fact]
        public void NotTextWorks()
        {
            var parser = IsDigit.NotText().CompileParser("NotTextWorks");
            parser.AssertSuccess("0", new StringSpan("0", 0, 0), 0);
            parser.AssertSuccess("", new StringSpan("", 0, 0), 0);
            parser.AssertSuccess("aaa1", new StringSpan("aaa1", 0, 3), 3);
            parser.AssertSuccess("aaa", new StringSpan("aaa", 0, 3), 3);
        }

        [Fact]
        public void RequiredWorks()
        {
            var parser = IsDigitText.Required().CompileParser("RequiredWorks");
            parser.AssertSuccess("0", new StringSpan("0", 0, 1), 1);
            parser.AssertFailure("", 0);
            parser.AssertFailure("a", 0);
            parser.AssertSuccess("000", new StringSpan("000", 0, 3), 3);
        }
    }
}
