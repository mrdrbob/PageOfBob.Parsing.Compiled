using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class ThenIgnoreRule<L, R> : AbstractThenRule<L, R, L>
    {
        public ThenIgnoreRule(IRule<L> leftRule, IRule<R> rightRule, string name) : base(leftRule, rightRule)
        {
            Name = $"( {leftRule.Name} then ignore {rightRule.Name} )";
        }

        protected override void EmitTransform<TDelegate>(CompilerContext<TDelegate> context, Local pos) // lv, rv
        {
            context.Emit.Pop();
        }

        public override string Name { get; }
    }
}
