using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public abstract class AbstractRuleWithEndCheck<T> : IRule<T>
    {
        protected abstract void EmitLogic<TDelegate>(CompilerContext<TDelegate> context, Label success);

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;

            emit.Duplicate(); // pos, pos
            emit.LoadArgument(1); // pos, pos, str
            emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // pos, pos, len
            var fail = emit.DefineLabel();
            emit.BranchIfGreaterOrEqual(fail); // pos

            // Otherwise, success!
            EmitLogic(context, success);

            emit.MarkLabel(fail); // pos
            return true;
        }

        public abstract string Name { get; }
    }
}
