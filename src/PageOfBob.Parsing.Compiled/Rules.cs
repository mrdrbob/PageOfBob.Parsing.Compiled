/*
using PageOfBob.Parsing.Compiled.GeneralRules;
using PageOfBob.Parsing.Compiled.StringRules;
using System;
using System.Collections.Generic;

namespace PageOfBob.Parsing.Compiled
{
    public static class Rules
    {
        public static IRule<char> Match(params char[] c) => new MatchCharRule(c);
        public static IRule<StringSpan> MatchSpan(params char[] c) => new MatchCharSpanRule(c);
        public static IRule<StringSpan> MatchSpanMany(Func<char, bool> match, string name = null) => new MatchSpanRule(match, name);
        public static IRule<char> IMatch(params char[] c) => new MatchCharInsensitiveRule(c);
        public static readonly IRule<char> IsLetter = new IsCharacterTypeMatchRule("IsLetter");
        public static readonly IRule<char> IsDigit = new IsCharacterTypeMatchRule("IsDigit");
        public static readonly IRule<char> IsLetterOrDigit = new IsCharacterTypeMatchRule("IsLetterOrDigit");
        public static readonly IRule<char> IsWhiteSpace = new IsCharacterTypeMatchRule("IsWhiteSpace");
        public static readonly IRule<int> GetPosition = new GetPositionRule();
        public static IRule<string> ManyAsText(this IRule<char> rule, string name = null) => new ManyAsTextRule(rule, name);
        public static IRule<StringSpan> ManyAsSpan(this IRule<StringSpan> rule, string name = null) => new ManySpanRule(rule, name);
        public static IRule<string> Required(this IRule<string> rule, string name = null) => new StringRequiredRule(rule, name);
        public static IRule<StringSpan> Required(this IRule<StringSpan> rule, string name = null) => new SpanRequiredRule(rule, name);
        public static IRule<char> Not<T>(this IRule<T> rule, string name = null) => new NotRule<T>(rule, name);
        public static IRule<StringSpan> NotSpan<T>(this IRule<T> rule, string name = null) => new NotSpanRule<T>(rule, name);
        public static IRule<string> Text(string expectedValue, bool caseInsensitive = false, string name = null) => new TextRule(expectedValue, caseInsensitive, name);

        public static IRule<T> Any<T>(params IRule<T>[] rules) => new AnyRule<T>(rules);
        public static IRule<O> Map<T, O>(this IRule<T> rule, Func<T, O> map, string name = null) => new MapRule<T, O>(rule, map, name);
        public static IRule<List<T>> Many<T>(this IRule<T> rule, bool keepResults = true, string name = null) => new ManyRule<T>(rule, keepResults, name);
        public static IRule<List<T>> Required<T>(this IRule<List<T>> rule, string name = null) => new RequiredRule<T>(rule, name);
        public static IRule<O> Then<L, R, O>(this IRule<L> left, IRule<R> right, Func<L, R, O> map, string name = null) => new ThenDoRule<L, R, O>(left, right, map, name);
        public static IRule<R> ThenKeep<L, R>(this IRule<L> left, IRule<R> right, string name = null) => new ThenKeepRule<L, R>(left, right, name);
        public static IRule<L> ThenIgnore<L, R>(this IRule<L> left, IRule<R> right, string name = null) => new ThenIgnoreRule<L, R>(left, right, name);
        public static IRule<T> Optional<T>(this IRule<T> rule, T defaultValue) => new OptionalRule<T>(rule, defaultValue);
        public static IRule<T> When<T>(this IRule<T> rule, Func<T, bool> condition, string conditionName = null) => new WhenRule<T>(rule, condition, conditionName);
    }
}
*/