using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class TextMatchRule : AbstractRules.AbstractTextMatchRule<StringSpan>
    {
        public TextMatchRule(string textToMatch, bool caseSensitive, string name = null) : base(textToMatch, caseSensitive, name) { }

        protected override void EmitLoadSuccess<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPos, string textToMatch)
        {
            context.EmitLoadStringSpan(pos, originalPos);
        }
    }
}
