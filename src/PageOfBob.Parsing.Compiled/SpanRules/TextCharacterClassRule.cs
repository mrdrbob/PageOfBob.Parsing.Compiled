using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class TextCharacterClassRule : AbstractRules.AbstractTextCharacterClassRule<StringSpan>
    {
        public TextCharacterClassRule(string charClass, string name = null) : base(charClass, name) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.EmitLoadStringSpan(pos, originalPosition);
        }
    }
}
