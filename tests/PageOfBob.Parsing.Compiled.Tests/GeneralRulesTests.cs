using Xunit;
using static PageOfBob.Parsing.Compiled.Rules;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class GeneralRulesTests
    {
        [Fact]
        public void AnyWorks()
        {
            var parser = Any(IsLetter, IsWhiteSpace).CompileParser("AnyWorks");
            parser.AssertSuccess("a", 'a', 1);
            parser.AssertSuccess(" ", ' ', 1);
            parser.AssertFailure("1", 0);
        }

        [Fact]
        public void AnyWorksWithRulesThatCantFail()
        {
            var parser = Any(
                    Match('a').Many(),
                    Match('b').Many())
                .Map(x => new string(x.ToArray()))
                .CompileParser("AnyWorksWithRulesThatCantFail");

            parser.AssertSuccess("aaabb", "aaa", 3);
            parser.AssertSuccess("bbb", "", 0);
            parser.AssertSuccess("z", "", 0);
        }

        [Fact]
        public void MapWorks()
        {
            var parser = Match('a', 'b', 'c').Map(char.ToUpperInvariant).CompileParser("MapWorks");
            parser.AssertSuccess("a", 'A', 1);
            parser.AssertSuccess("b", 'B', 1);
            parser.AssertFailure("e", 0);
        }

        [Fact]
        public void ManyWorks()
        {
            var parser = Match('a').Many().Map(x => new string(x.ToArray())).CompileParser("ManyWorks");
            parser.AssertSuccess("aaa", "aaa", 3);
            parser.AssertSuccess("b", "", 0);
        }

        [Fact]
        public void ManyNotKeepWorks()
        {
            var parser = Match('a').Many(false).CompileParser("ManyNotKeepWorks");
            parser.AssertSuccessNull("aaa", 3);
            parser.AssertSuccessNull("b", 0);
        }

        [Fact]
        public void RequiredWorks()
        {
            var parser = Match('a').Many().Required().Map(x => new string(x.ToArray())).CompileParser("RequiredWorks");
            parser.AssertSuccess("aaa", "aaa", 3);
            parser.AssertFailure("b", 0);
        }

        [Fact]
        public void ThenKeepWorks()
        {
            var parser = Match('a').ThenKeep(Match('b')).CompileParser("ThenKeepWorks");
            parser.AssertSuccess("ab", 'b', 2);
            parser.AssertFailure("ac", 0);
            parser.AssertFailure("ba", 0);
        }

        [Fact]
        public void ThenIgnoreWorks()
        {
            var parser = Match('a').ThenIgnore(Match('b')).CompileParser("ThenIgnoreWork");
            parser.AssertSuccess("ab", 'a', 2);
            parser.AssertFailure("ac", 0);
            parser.AssertFailure("ba", 0);
        }

        [Fact]
        public void ThenWorks()
        {
            var parser = Match('a').Then(Match('b'), (x,y) => new string(new[] { y, x })).CompileParser("ThenWorks");
            parser.AssertSuccess("ab", "ba", 2);
            parser.AssertFailure("ac", 0);
            parser.AssertFailure("ba", 0);
        }

        [Fact]
        public void ThenWorksWhenNeitherRuleCanFail()
        {
            var parser = Match('a').Many()
                .ThenKeep(Match('b').Many())
                .Map(x => new string(x.ToArray()))
                .CompileParser("ThenWorksWhenNeitherRuleCanFail");

            parser.AssertSuccess("aaabb", "bb", 5);
            parser.AssertSuccess("aaa", "", 3);
            parser.AssertSuccess("bbb", "bbb", 3);
            parser.AssertSuccess("z", "", 0);
        }
    }
}
