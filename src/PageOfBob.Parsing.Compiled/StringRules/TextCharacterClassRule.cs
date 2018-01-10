using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class TextCharacterClassRule : AbstractRules.AbstractTextCharacterClassRule<string>
    {
        public TextCharacterClassRule(string charClass, string name = null) : base(charClass, name) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.EmitLoadString(pos, originalPosition);
        }
    }
}
