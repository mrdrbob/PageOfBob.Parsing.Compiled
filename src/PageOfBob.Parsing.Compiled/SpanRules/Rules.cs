using System;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public static class Rules
    {
        public static IRule<StringSpan> Match(params char[] charsToMatch) => new MatchCharRule(true, charsToMatch);
        public static IRule<StringSpan> IMatch(params char[] charsToMatch) => new MatchCharRule(false, charsToMatch);
        public static IRule<StringSpan> Match(Func<char, bool> match, string name = null) => new MatchFunctionRule(match, null);
        public static IRule<StringSpan> Not<K>(this IRule<K> rule, string name = null) => new NotRule<K>(rule, name);

        public static readonly IRule<StringSpan> IsControl = new MatchCharacterClassRule("IsControl");
        public static readonly IRule<StringSpan> IsDigit = new MatchCharacterClassRule("IsDigit");
        public static readonly IRule<StringSpan> IsLetter = new MatchCharacterClassRule("IsLetter");
        public static readonly IRule<StringSpan> IsLetterOrDigit = new MatchCharacterClassRule("IsLetterOrDigit");
        public static readonly IRule<StringSpan> IsLower = new MatchCharacterClassRule("IsLower");
        public static readonly IRule<StringSpan> IsNumber = new MatchCharacterClassRule("IsNumber");
        public static readonly IRule<StringSpan> IsPunctuation = new MatchCharacterClassRule("IsPunctuation");
        public static readonly IRule<StringSpan> IsSeparator = new MatchCharacterClassRule("IsSeparator");
        public static readonly IRule<StringSpan> IsSymbol = new MatchCharacterClassRule("IsSymbol");
        public static readonly IRule<StringSpan> IsUpper = new MatchCharacterClassRule("IsUpper");
        public static readonly IRule<StringSpan> IsWhiteSpace = new MatchCharacterClassRule("IsWhiteSpace");

        public static IRule<StringSpan> Text(params char[] charsToMatch) => new TextMatchCharRule(true, charsToMatch);
        public static IRule<StringSpan> IText(params char[] charsToMatch) => new TextMatchCharRule(false, charsToMatch);
        public static IRule<StringSpan> Text(string textToMatch, string name = null) => new TextMatchRule(textToMatch, true, name);
        public static IRule<StringSpan> IText(string textToMatch, string name = null) => new TextMatchRule(textToMatch, false, name);
        public static IRule<StringSpan> Text(Func<char, bool> match, string name = null) => new TextFuncRule(match, name);
        public static IRule<StringSpan> NotText<K>(this IRule<K> rule, string name = null) => new NotTextRule<K>(rule, name);
        public static IRule<StringSpan> Required(this IRule<StringSpan> rule) => new RequiredRule(rule);
        public static IRule<StringSpan> ThenCreateSpan(this IRule<int> rule, string name = null) => new ThenCreateSpanRule(rule, name);

        public static readonly IRule<StringSpan> IsControlText = new TextCharacterClassRule("IsControl");
        public static readonly IRule<StringSpan> IsDigitText = new TextCharacterClassRule("IsDigit");
        public static readonly IRule<StringSpan> IsLetterText = new TextCharacterClassRule("IsLetter");
        public static readonly IRule<StringSpan> IsLetterOrDigitText = new TextCharacterClassRule("IsLetterOrDigit");
        public static readonly IRule<StringSpan> IsLowerText = new TextCharacterClassRule("IsLower");
        public static readonly IRule<StringSpan> IsNumberText = new TextCharacterClassRule("IsNumber");
        public static readonly IRule<StringSpan> IsPunctuationText = new TextCharacterClassRule("IsPunctuation");
        public static readonly IRule<StringSpan> IsSeparatorText = new TextCharacterClassRule("IsSeparator");
        public static readonly IRule<StringSpan> IsSymbolText = new TextCharacterClassRule("IsSymbol");
        public static readonly IRule<StringSpan> IsUpperText = new TextCharacterClassRule("IsUpper");
        public static readonly IRule<StringSpan> IsWhiteSpaceText = new TextCharacterClassRule("IsWhiteSpace");
    }
}
