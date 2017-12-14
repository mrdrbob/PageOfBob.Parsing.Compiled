using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class GetPositionRule : IRule<int>
    {
        public string Name => "Get Position";

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            // p
            context.Emit.Duplicate(); // p, p
            context.Emit.Branch(success);
            return false;
        }
    }
}
