using Xunit;
using static PageOfBob.Parsing.Compiled.StringRules.Rules;
using static PageOfBob.Parsing.Compiled.GeneralRules.Rules;

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


        [Fact]
        public void ThenCreateSpanWorks()
        {
            var parser = GetPosition
                .ThenIgnore(Text('a').Required())
                .ThenIgnore(Text('b').Required())
                .ThenCreateString()
                .CompileParser("ThenCreateSpanWorks");
            parser.AssertSuccess("aabbcc", "aabb", 4);
            parser.AssertSuccess("abab", "ab", 2);
            parser.AssertFailure("c", 0);
        }
    }
}
