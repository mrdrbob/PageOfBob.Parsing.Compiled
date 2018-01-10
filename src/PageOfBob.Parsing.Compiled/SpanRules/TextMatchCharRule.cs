using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class TextMatchCharRule : AbstractRules.AbstractTextCharRule<StringSpan>
    {
        public TextMatchCharRule(bool caseSensitive, params char[] charsToMatch) : base(caseSensitive, charsToMatch) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.EmitLoadStringSpan(pos, originalPosition);
        }
    }
}
