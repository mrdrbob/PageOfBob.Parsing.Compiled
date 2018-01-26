using Sigil;
using System;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class ThenDoRule<L, R, O> : AbstractThenRule<L, R, O>
    {
        private readonly Func<L, R, O> transform;
        private readonly string name;

        public ThenDoRule(IRule<L> leftRule, IRule<R> rightRule, Func<L, R, O> transform, string name) : base(leftRule, rightRule)
        {
            this.transform = transform;
            this.name = name;
        }

        public override string Name => name;

        protected override void EmitTransform<TDelegate>(CompilerContext<TDelegate> context, Local pos) //lv, rv
        {
            var field = context.SaveInField(transform);
            var emit = context.Emit;

            using (var lv = emit.DeclareLocal<L>())
            using (var rv = emit.DeclareLocal<R>())
            {
                emit.StoreLocal(rv); // lv
                emit.StoreLocal(lv); // ...
                emit.LoadField(field); // action
                emit.LoadLocal(lv); //action, lv
                emit.LoadLocal(rv); //action, lv, rv
                emit.CallVirtual(typeof(Func<L, R, O>).GetMethod("Invoke", new[] { typeof(L), typeof(R) })); // nv
            }

        }
    }
}