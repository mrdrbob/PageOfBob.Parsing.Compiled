using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class NotTextRule<K> : AbstractRules.AbstractNotTextRule<string, K>
    {
        public NotTextRule(IRule<K> rule, string name) : base(rule, name) { }

        protected override void EmitLoadSuccess<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPos)
        {
            context.EmitLoadString(pos, originalPos);
        }
    }
}
