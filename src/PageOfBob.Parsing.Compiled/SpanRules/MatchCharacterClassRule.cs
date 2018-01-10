using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class MatchCharacterClassRule : AbstractRules.AbstractMatchCharClassRule<StringSpan>
    {
        public MatchCharacterClassRule(string charClass, string name = null) : base(charClass, name) { }

        protected override void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadStringSpan(pos);
        }
    }
}
