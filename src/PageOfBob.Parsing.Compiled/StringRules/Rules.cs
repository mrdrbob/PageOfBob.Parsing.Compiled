using System;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public static class Rules
    {
        public static IRule<char> Match(params char[] charsToMatch) => new MatchCharRule(true, charsToMatch);
        public static IRule<char> IMatch(params char[] charsToMatch) => new MatchCharRule(false, charsToMatch);
        public static IRule<char> Match(Func<char, bool> match, string name = null) => new MatchFunctionRule(match, null);
        public static IRule<char> Not<K>(this IRule<K> rule, string name = null) => new NotRule<K>(rule, name);

        public static readonly IRule<char> IsControl = new MatchCharacterClassRule("IsControl");
        public static readonly IRule<char> IsDigit = new MatchCharacterClassRule("IsDigit");
        public static readonly IRule<char> IsLetter = new MatchCharacterClassRule("IsLetter");
        public static readonly IRule<char> IsLetterOrDigit = new MatchCharacterClassRule("IsLetterOrDigit");
        public static readonly IRule<char> IsLower = new MatchCharacterClassRule("IsLower");
        public static readonly IRule<char> IsNumber = new MatchCharacterClassRule("IsNumber");
        public static readonly IRule<char> IsPunctuation = new MatchCharacterClassRule("IsPunctuation");
        public static readonly IRule<char> IsSeparator = new MatchCharacterClassRule("IsSeparator");
        public static readonly IRule<char> IsSymbol = new MatchCharacterClassRule("IsSymbol");
        public static readonly IRule<char> IsUpper = new MatchCharacterClassRule("IsUpper");
        public static readonly IRule<char> IsWhiteSpace = new MatchCharacterClassRule("IsWhiteSpace");

        public static IRule<string> Text(params char[] charsToMatch) => new TextMatchCharRule(true, charsToMatch);
        public static IRule<string> IText(params char[] charsToMatch) => new TextMatchCharRule(false, charsToMatch);
        public static IRule<string> Text(string textToMatch, string name = null) => new TextMatchRule(textToMatch, true, true, name);
        public static IRule<string> IText(string textToMatch, bool keepMatched = false, string name = null) => new TextMatchRule(textToMatch, false, !keepMatched, name);
        public static IRule<string> Text(Func<char, bool> match, string name = null) => new TextFuncRule(match, name);
        public static IRule<string> NotText<K>(this IRule<K> rule, string name = null) => new NotTextRule<K>(rule, name);
        public static IRule<string> Required(this IRule<string> rule) => new RequiredRule(rule);

        public static readonly IRule<string> IsControlText = new TextCharacterClassRule("IsControl");
        public static readonly IRule<string> IsDigitText = new TextCharacterClassRule("IsDigit");
        public static readonly IRule<string> IsLetterText = new TextCharacterClassRule("IsLetter");
        public static readonly IRule<string> IsLetterOrDigitText = new TextCharacterClassRule("IsLetterOrDigit");
        public static readonly IRule<string> IsLowerText = new TextCharacterClassRule("IsLower");
        public static readonly IRule<string> IsNumberText = new TextCharacterClassRule("IsNumber");
        public static readonly IRule<string> IsPunctuationText = new TextCharacterClassRule("IsPunctuation");
        public static readonly IRule<string> IsSeparatorText = new TextCharacterClassRule("IsSeparator");
        public static readonly IRule<string> IsSymbolText = new TextCharacterClassRule("IsSymbol");
        public static readonly IRule<string> IsUpperText = new TextCharacterClassRule("IsUpper");
        public static readonly IRule<string> IsWhiteSpaceText = new TextCharacterClassRule("IsWhiteSpace");
    }
}
