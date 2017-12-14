using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class IsCharacterTypeMatchRule : AbstractMatchRule
    {
        private readonly string methodName;

        public IsCharacterTypeMatchRule(string methodName)
        {
            this.methodName = methodName;
        }

        public override string Name => methodName;

        protected override void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label match)
        {
            context.Emit.Duplicate(); // v2, v2
            context.Emit.Call(typeof(char).GetMethod(methodName, new[] { typeof(char) })); // v2, bool
            context.Emit.BranchIfTrue(match); // v2

            // Failed.
            context.Emit.Pop(); // ...
        }
    }
}
