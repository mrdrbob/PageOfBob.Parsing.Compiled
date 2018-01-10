using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class MatchCharacterClassRule : AbstractRules.AbstractMatchCharClassRule<char>
    {
        public MatchCharacterClassRule(string charClass, string name = null) : base(charClass, name) { }

        protected override void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadChar(pos);
        }
    }
}
