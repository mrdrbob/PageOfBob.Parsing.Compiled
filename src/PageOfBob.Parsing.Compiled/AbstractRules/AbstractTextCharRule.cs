using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractTextCharRule<T> : AbstractTextRule<T>
    {
        private readonly bool caseSensitive;
        private readonly char[] charsToMatch;

        protected AbstractTextCharRule(bool caseSensitive, params char[] charsToMatch)
        {
            this.caseSensitive = caseSensitive;
            this.charsToMatch = charsToMatch;
            Name = "(" + string.Join(",", charsToMatch) + ")";
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label matchSuccess)
        {
            context.MatchCharacters(caseSensitive, charsToMatch, matchSuccess);
        }
    }
}
