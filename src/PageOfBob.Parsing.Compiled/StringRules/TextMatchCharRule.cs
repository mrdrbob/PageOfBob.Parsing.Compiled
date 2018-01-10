using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class TextMatchCharRule : AbstractRules.AbstractTextCharRule<string>
    {
        public TextMatchCharRule(bool caseSensitive, params char[] charsToMatch) : base(caseSensitive, charsToMatch) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.EmitLoadString(pos, originalPosition);
        }
    }
}
