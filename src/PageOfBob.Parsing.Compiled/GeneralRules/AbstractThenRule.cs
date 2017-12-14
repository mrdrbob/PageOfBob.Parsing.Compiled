using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public abstract class AbstractThenRule<L, R, O> : IRule<O>
    {
        protected readonly IRule<L> leftRule;
        protected readonly IRule<R> rightRule;

        public abstract string Name { get; }

        public AbstractThenRule(IRule<L> leftRule, IRule<R> rightRule)
        {
            this.leftRule = leftRule;
            this.rightRule = rightRule;
        }

        protected abstract void EmitTransform<TDelegate>(CompilerContext<TDelegate> context);

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var end = emit.DefineLabel();
            var leftSuccess = emit.DefineLabel();

            bool leftCanFail;
            bool rightCanFail;

            using (var pos = emit.DeclareLocal<int>())
            {
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(pos); // pos

                leftCanFail = leftRule.Emit(context, leftSuccess);
                if (leftCanFail)
                {
                    // Left failed!
                    emit.Branch(end); //pos
                }

                // Left sucecss:
                // lv, pos2
                emit.MarkLabel(leftSuccess);
                var rightSuccess = emit.DefineLabel();
                rightCanFail = rightRule.Emit(context, rightSuccess);
                if (rightCanFail)
                {
                    // Right failed!
                    emit.Pop(); // lv
                    emit.Pop(); // ...
                    emit.LoadLocal(pos); // pos
                    emit.Branch(end);
                }

                emit.MarkLabel(rightSuccess);
                // Right success:
                // lv, rv, pos3
                emit.StoreLocal(pos); // lv, rv
                EmitTransform(context); // Should return nv
                emit.LoadLocal(pos);  // nv, pos3
                emit.Branch(success);

                if (leftCanFail || rightCanFail)
                {
                    emit.MarkLabel(end);
                }
            }

            return leftCanFail || rightCanFail;
        }
    }
}
