using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractNotRule<T, K> : AbstractRuleWithEndCheck<T>
    {
        private readonly IRule<K> rule;

        public AbstractNotRule(IRule<K> rule, string name = null)
        {
            this.rule = rule;
            Name = name ?? $"!{rule.Name}";
        }

        public override string Name { get; }

        protected abstract void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos);

        protected override void EmitLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var localSuccess = emit.DefineLabel();

            // pos
            emit.Duplicate(); // pos, pos

            bool canFail = rule.Emit(context, localSuccess);
            if (canFail)
            {
                using (var pos = emit.DeclareLocal<int>())
                {
                    // A failed rule is success
                    // pos, pos
                    emit.Pop(); // pos
                    emit.StoreLocal(pos); // ...

                    EmitSuccessLogic(context, pos); // v

                    emit.LoadLocal(pos); // v, pos
                    emit.LoadConstant(1); // v, pos, 1
                    emit.Add(); // v, pos+1
                    emit.Branch(success);
                }
            }

            emit.MarkLabel(localSuccess); // opos, v2, pos
            emit.Pop(); // opos, v2
            emit.Pop(); // opos

            // Fail
        }
    }
}
