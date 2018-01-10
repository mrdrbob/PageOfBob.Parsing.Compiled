using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractCheckedRule<T> : IRule<T>
    {
        private readonly IRule<T> rule;

        protected AbstractCheckedRule(IRule<T> rule, string name = null)
        {
            this.rule = rule;
            Name = name ?? $"{rule.Name}.Required()";
        }

        public string Name { get; }

        protected abstract void EmitCheck<TDelegate>(CompilerContext<TDelegate> context, Label checkFail); // v

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var end = emit.DefineLabel();
            var requireFail = emit.DefineLabel();

            var localSuccess = emit.DefineLabel();

            using (var oPos = emit.DeclareLocal<int>())
            {
                // pos
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(oPos); // pos

                bool canFail = rule.Emit(context, localSuccess);
                if (canFail)
                {
                    emit.Branch(end);
                }

                // Success
                emit.MarkLabel(localSuccess); // v, pos
                using (var pos = emit.DeclareLocal<int>())
                {
                    emit.StoreLocal(pos); // v
                    emit.Duplicate(); // v, v
                    EmitCheck(context, requireFail); // v

                    // Success
                    emit.LoadLocal(pos); // v, pos
                    emit.Branch(success);

                    // Check fail
                    emit.MarkLabel(requireFail); // v
                    emit.Pop(); // ...
                    emit.LoadLocal(oPos); // pos
                }

            }
            emit.MarkLabel(end);
            return true;
        }
    }
}
