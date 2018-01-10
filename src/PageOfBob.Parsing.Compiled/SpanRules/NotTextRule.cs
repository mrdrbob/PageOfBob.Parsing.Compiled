using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class NotTextRule<K> : AbstractRules.AbstractNotTextRule<StringSpan, K>
    {
        public NotTextRule(IRule<K> rule, string name) : base(rule, name) { }

        protected override void EmitLoadSuccess<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPos)
        {
            context.EmitLoadStringSpan(pos, originalPos);
        }
    }
}
