using Xunit;
using static PageOfBob.Parsing.Compiled.StringRules.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class StringRulesTests
    {
        [Fact]
        public void MatchMatchesChars()
        {
            var parser = Match('a', 'b', 'c').CompileParser("StringMatchMatchesChars");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("b", 'b', 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void IMatchMatchesChars()
        {
            var parser = IMatch('a', 'b', 'c').CompileParser("StringIMatchMatchesChars");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertSuccess("A", 'A', 1);
            parser.AssertSuccess("b", 'b', 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void IsDigitWorks()
        {
            var parser = IsDigit.CompileParser("StringIsDigitWorks");
            parser.AssertSuccess("0", '0', 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("9", '9', 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void MatchFuncWorks()
        {
            var parser = Match(char.IsDigit).CompileParser("StringMatchFuncWorks");
            parser.AssertSuccess("0", '0', 1);
            parser.AssertFailure("A", 0);
            parser.AssertSuccess("9", '9', 1);
            parser.AssertFailure("f", 0);
        }

        [Fact]
        public void NotWorks()
        {
            var parser = IsDigit.Not().CompileParser("StringNotWorks");
            parser.AssertFailure("0", 0);
            parser.AssertFailure("", 0);
            parser.AssertSuccess("a", 'a', 1);
        }

        [Fact]
        public void MatchMatchesCharsText()
        {
            var parser = Text('a', 'b', 'c').CompileParser("StringMatchMatchesCharsText");
            parser.AssertSuccess("a", "a", 1);
            parser.AssertSuccess("A", "", 0);
            parser.AssertSuccess("bca3", "bca", 3);
        }

        [Fact]
        public void IMatchMatchesCharsText()
        {
            var parser = IText('a', 'b', 'c').CompileParser("StringIMatchMatchesCharsText");
            parser.AssertSuccess("a", "a", 1);
            parser.AssertSuccess("A", "A", 1);
            parser.AssertSuccess("Z", "", 0);
            parser.AssertSuccess("bca3", "bca", 3);
        }

        [Fact]
        public void IsDigitWorksText()
        {
            var parser = IsDigitText.CompileParser("StringIsDigitWorksText");
            parser.AssertSuccess("000", "000", 3);
            parser.AssertSuccess("A", "", 0);
            parser.AssertSuccess("909a", "909", 3);
        }

        [Fact]
        public void TextFuncWorks()
        {
            var parser = Text(char.IsDigit).CompileParser("StringTextFuncWorks");
            parser.AssertSuccess("000", "000", 3);
            parser.AssertSuccess("A", "", 0);
            parser.AssertSuccess("909a", "909", 3);
        }

        [Fact]
        public void TextWorks()
        {
            var parser = Text("abc").CompileParser("StringTextWorks");
            parser.AssertFailure("", 0);
            parser.AssertFailure("ab", 0);
            parser.AssertFailure("ABC", 0);
            parser.AssertSuccess("abc", "abc", 3);
            parser.AssertSuccess("abcefg", "abc", 3);
        }

        [Fact]
        public void ITextWorks()
        {
            var parser = IText("abc").CompileParser("StringITextWorks");
            parser.AssertFailure("", 0);
            parser.AssertFailure("ab", 0);
            parser.AssertSuccess("ABC", "abc", 3);
            parser.AssertSuccess("abc", "abc", 3);
            parser.AssertSuccess("abcefg", "abc", 3);
        }

        [Fact]
        public void ITextCanKeepOriginal()
        {
            var parser = IText("abc", true).CompileParser("StringITextCanKeepOriginal");
            parser.AssertFailure("", 0);
            parser.AssertFailure("ab", 0);
            parser.AssertSuccess("ABC", "ABC", 3);
            parser.AssertSuccess("abc", "abc", 3);
            parser.AssertSuccess("aBcefg", "aBc", 3);
        }

        [Fact]
        public void NotTextWorks()
        {
            var parser = IsDigit.NotText().CompileParser("StringNotTextWorks");
            parser.AssertSuccess("0", "", 0);
            parser.AssertSuccess("", "", 0);
            parser.AssertSuccess("aaa1", "aaa", 3);
            parser.AssertSuccess("aaa", "aaa", 3);
        }

        [Fact]
        public void RequiredWorks()
        {
            var parser = IsDigitText.Required().CompileParser("StringRequiredWorks");
            parser.AssertSuccess("0", "0", 1);
            parser.AssertFailure("", 0);
            parser.AssertFailure("a", 0);
            parser.AssertSuccess("000", "000", 3);
        }

        /*
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
        public void MatchSpanManyWorks()
        {
            var parser = MatchSpanMany(char.IsLetter).CompileParser("MatchSpanManyWorks");
            parser.AssertSuccess("abc3", new StringSpan("abc3", 0, 3), 3);
            parser.AssertSuccess("", new StringSpan("", 0, 0), 0);
            parser.AssertSuccess("abc", new StringSpan("abc", 0, 3), 3);

            var result = parser.TryParse("abc3", out StringSpan span, out int position);
            Assert.True(result);
            Assert.True(span.Matches("abc"));
        }

        [Fact]
        public void CanConcatSequentialSpans()
        {
            var parser = MatchSpanMany(char.IsLetter)
                .Then(MatchSpanMany(char.IsDigit), StringSpanExtensions.CombineSequential)
                .CompileParser("CanConcatSequentialSpans");
            parser.AssertSuccess("ab12cc", new StringSpan("ab12cc", 0, 4), 4);
            parser.AssertSuccess("ab", new StringSpan("ab", 0, 2), 2);
            parser.AssertSuccess("34", new StringSpan("34", 0, 2), 2);
            parser.AssertSuccess("!", new StringSpan("!", 0, 0), 0);
        }

        [Fact]
        public void MatchSpanRequiredWorks()
        {
            var parser = MatchSpanMany(char.IsLetter).Required().CompileParser("MatchSpanRequiredWorks");
            parser.AssertSuccess("abc3", new StringSpan("abc3", 0, 3), 3);
            parser.AssertFailure("3", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void ManyAsSpanWorks()
        {
            var parser = MatchSpan('a', 'b').ManyAsSpan().CompileParser("ManyAsSpanWorks");
            parser.AssertSuccess("abc", new StringSpan("abc", 0, 2), 2);
            parser.AssertSuccess("c", new StringSpan("c", 0, 0), 0);
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
        public void NotNegatesRuleSpan()
        {
            var parser = Match('a', 'b', 'c').NotSpan().CompileParser("NotNegatesRuleSpan");
            parser.AssertFailure("a", 0);
            parser.AssertFailure("", 0);
            parser.AssertSuccess("d", new StringSpan("d", 0, 1), 1);
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
        */
    }
}
