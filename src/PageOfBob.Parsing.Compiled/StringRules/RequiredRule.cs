using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class RequiredRule : AbstractRules.AbstractCheckedRule<string>
    {
        public RequiredRule(IRule<string> rule, string name = null) : base(rule, name) { }

        protected override void EmitCheck<TDelegate>(CompilerContext<TDelegate> context, Label checkFail)
        {
            // v
            context.Emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // len
            context.Emit.BranchIfFalse(checkFail);
        }
    }
}
