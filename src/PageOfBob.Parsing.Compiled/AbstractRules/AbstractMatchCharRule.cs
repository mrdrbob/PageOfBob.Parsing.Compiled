using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractMatchCharRule<T> : AbstractMatchRule<T>
    {
        private readonly bool caseSensitive;
        private readonly char[] charsToMatch;

        protected AbstractMatchCharRule(bool caseSensitive, params char[] charsToMatch)
        {
            this.caseSensitive = caseSensitive;
            this.charsToMatch = charsToMatch;
            Name = "(" + string.Join(",", charsToMatch) + ")";
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            context.MatchCharacters(caseSensitive, charsToMatch, success);
        }
    }
}
