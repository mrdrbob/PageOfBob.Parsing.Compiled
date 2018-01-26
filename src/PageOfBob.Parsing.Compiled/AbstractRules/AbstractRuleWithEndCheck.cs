using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractRuleWithEndCheck<T> : IRule<T>
    {
        public abstract string Name { get; }

        protected abstract void EmitLogic<TDelegate>(CompilerContext<TDelegate> context, Label success);

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;

            emit.Duplicate(); // pos, pos
            emit.LoadLocal(context.LengthLocal); // pos, pos, len
            var fail = emit.DefineLabel();
            emit.BranchIfGreaterOrEqual(fail); // pos

            // Otherwise, success!
            EmitLogic(context, success);

            emit.MarkLabel(fail); // pos
            return true;
        }
    }
}
