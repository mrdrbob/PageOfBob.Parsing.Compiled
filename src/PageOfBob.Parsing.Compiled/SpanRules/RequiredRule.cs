using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class RequiredRule : AbstractRules.AbstractCheckedRule<StringSpan>
    {
        public RequiredRule(IRule<StringSpan> rule, string name = null) : base(rule, name) { }

        protected override void EmitCheck<TDelegate>(CompilerContext<TDelegate> context, Label checkFail)
        {
            using (var t = context.Emit.DeclareLocal<StringSpan>())
            {
                context.Emit.StoreLocal(t); // ...
                context.Emit.LoadLocalAddress(t); // t*
                context.Emit.CallVirtual(typeof(StringSpan).GetProperty("Length").GetMethod); // Length
                context.Emit.BranchIfFalse(checkFail);
            }
        }
    }
}
