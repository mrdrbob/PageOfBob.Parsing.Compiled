namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class ThenKeepRule<L, R> : AbstractThenRule<L, R, R>
    {
        public ThenKeepRule(IRule<L> leftRule, IRule<R> rightRule, string name) : base(leftRule, rightRule)
        {
            Name = name ?? $"( {leftRule.Name} then keep {rightRule.Name} )";
        }

        public override string Name { get; }

        protected override void EmitTransform<TDelegate>(CompilerContext<TDelegate> context) // lv, rv
        {
            var emit = context.Emit;
            using (var rightValue = emit.DeclareLocal<R>())
            {
                emit.StoreLocal(rightValue); // lv
                emit.Pop();  // ...
                emit.LoadLocal(rightValue); // rv
            }
        }
    }
}
