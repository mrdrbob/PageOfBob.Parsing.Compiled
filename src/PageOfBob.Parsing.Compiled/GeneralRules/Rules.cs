using System;
using System.Collections.Generic;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public static class Rules
    {
        public static IRule<T> Always<T>(Func<T> generateMethod, string name = null) => new AlwaysRule<T>(generateMethod, name);
        public static IRule<T> Always<T>(T value, string name = null) => new AlwaysValueRule<T>(value, name);
        public static IRule<T> Any<T>(params IRule<T>[] rules) => new AnyRule<T>(rules);
        public static IRule<O> Map<T, O>(this IRule<T> rule, Func<T, O> map, string name = null) => new MapRule<T, O>(rule, map, name);
        public static IRule<List<T>> Many<T>(this IRule<T> rule, bool keepResults = true, string name = null) => new ManyRule<T, object>(rule, keepResults, null, name);
        public static IRule<List<T>> Many<T, K>(this IRule<T> rule, IRule<K> separator, bool keepResults = true, string name = null) => new ManyRule<T, K>(rule, keepResults, separator, name);
        public static IRule<O> Then<L, R, O>(this IRule<L> left, IRule<R> right, Func<L, R, O> map, string name = null) => new ThenDoRule<L, R, O>(left, right, map, name);
        public static IRule<R> ThenKeep<L, R>(this IRule<L> left, IRule<R> right, string name = null) => new ThenKeepRule<L, R>(left, right, name);
        public static IRule<L> ThenIgnore<L, R>(this IRule<L> left, IRule<R> right, string name = null) => new ThenIgnoreRule<L, R>(left, right, name);
        public static IRule<T> Optional<T>(this IRule<T> rule, T defaultValue) => Any(rule, Always(defaultValue));
        public static IRule<T> Optional<T>(this IRule<T> rule, Func<T> generateMethod) => Any(rule, Always(generateMethod));
        public static IRule<T> When<T>(this IRule<T> rule, Func<T, bool> condition, string conditionName = null) => new WhenRule<T>(rule, condition, conditionName);
        public static readonly IRule<int> GetPosition = new GetPositionRule();
    }
}
