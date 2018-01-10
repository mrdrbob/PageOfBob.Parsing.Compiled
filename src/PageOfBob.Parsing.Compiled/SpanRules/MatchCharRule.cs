using PageOfBob.Parsing.Compiled.AbstractRules;
using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class MatchCharRule : AbstractMatchCharRule<StringSpan>
    {
        public MatchCharRule(bool caseSensitive, params char[] charsToMatch) : base(caseSensitive, charsToMatch) { }

        protected override void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadStringSpan(pos);
        }
    }
}
