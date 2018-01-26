using Xunit;
using static PageOfBob.Parsing.Compiled.StringRules.Rules;
using static PageOfBob.Parsing.Compiled.GeneralRules.Rules;
using System;

namespace PageOfBob.Parsing.Compiled.Tests
{
    public class GeneralRulesTests
    {
        [Fact]
        public void AlwaysWorks()
        {
            int count = 0;
            int generate() => count++;

            var parser = Always(generate).CompileParser("AlwaysWorks");
            parser.AssertSuccess("", 0, 0);
            parser.AssertSuccess("test", 1, 0);
        }

        [Fact]
        public void AlwaysValueWorks()
        {
            var parser = Always(9).CompileParser("AlwaysValueWorks");
            parser.AssertSuccess("", 9, 0);
            parser.AssertSuccess("test", 9, 0);
        }

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
                    Text('a'),
                    Text('b'))
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
        public void ManyWithSepWorks()
        {
            var parser = Match('a').Many(Match(',')).Map(x => new string(x.ToArray())).CompileParser("ManyWithSepWorks");
            parser.AssertSuccess("a,a,a", "aaa", 5);
            parser.AssertSuccess("aaa", "a", 1);
            parser.AssertSuccess("b", "", 0);
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
            var parser = Text('a')
                .ThenKeep(Text('b'))
                .CompileParser("ThenWorksWhenNeitherRuleCanFail");

            parser.AssertSuccess("aaabb", "bb", 5);
            parser.AssertSuccess("aaa", "", 3);
            parser.AssertSuccess("bbb", "bbb", 3);
            parser.AssertSuccess("z", "", 0);
        }

        [Fact]
        public void PositionWorks()
        {
            var parser = GetPosition
                .ThenIgnore(Match('a'))
                .Then(GetPosition, (l, r) => $"{l} - {r}")
                .CompileParser("PositionWorks");

            parser.AssertSuccess("a", "0 - 1", 1);
        }

        [Fact]
        public void WhenWorks()
        {
            var parser = IMatch('a').When(char.IsUpper, "is capital")
                .CompileParser("WhenWorks");

            parser.AssertSuccess("A", 'A', 1);
            parser.AssertFailure("a", 0);
            parser.AssertFailure("b", 0);
            parser.AssertFailure("", 0);
        }

        [Fact]
        public void OptionalWorks()
        {
            var parser = Match('a').Optional('0')
                .ThenIgnore(Match('b'))
                .CompileParser("OptionalWorks");

            parser.AssertSuccess("ab", 'a', 2);
            parser.AssertSuccess("b", '0', 1);
        }
    }
}
