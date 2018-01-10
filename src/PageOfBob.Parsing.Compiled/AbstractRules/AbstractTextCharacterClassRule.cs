using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractTextCharacterClassRule<T> : AbstractRules.AbstractTextRule<T>
    {
        private readonly string charClass;

        protected AbstractTextCharacterClassRule(string charClass, string name = null)
        {
            this.charClass = charClass;
            Name = name;
        }

        public override string Name { get; }

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label matchSuccess)
        {
            // c
            context.MatchCharacterClass(charClass, matchSuccess);
        }
    }
}
