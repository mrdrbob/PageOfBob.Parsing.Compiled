using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class NotRule<K> : AbstractRules.AbstractNotRule<StringSpan, K>
    {
        public NotRule(IRule<K> rule, string name = null) : base(rule, name) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadStringSpan(pos);
        }
    }
}
