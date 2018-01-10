using System;
using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class WhenRule<T> : AbstractRules.AbstractCheckedRule<T>
    {
        private readonly Func<T, bool> check;

        public WhenRule(IRule<T> rule, Func<T, bool> check, string name = null) : base(rule, name)
        {
            this.check = check;
        }

        protected override void EmitCheck<TDelegate>(CompilerContext<TDelegate> context, Label checkFail) 
        {
            var emit = context.Emit;
            var field = context.SaveInField(check);
            using (var v = emit.DeclareLocal<T>())
            {
                emit.StoreLocal(v); // ...
                emit.LoadField(field); // chk
                emit.LoadLocal(v); // chk, v
                emit.CallVirtual(typeof(Func<T, bool>).GetMethod("Invoke", new[] { typeof(T) })); // bool
                emit.BranchIfFalse(checkFail); // ...
            }
        }
    }
}
