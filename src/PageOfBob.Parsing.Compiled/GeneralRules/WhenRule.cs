using System;
using System.Collections.Generic;
using System.Text;
using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class WhenRule<T> : IRule<T>
    {
        private readonly IRule<T> rule;
        private readonly Func<T, bool> condition;
        private readonly string conditionName;

        public WhenRule(IRule<T> rule,  Func<T, bool> condition, string conditionName = null)
        {
            this.rule = rule;
            this.condition = condition;
            this.conditionName = conditionName;
        }

        public string Name => conditionName == null ? rule.Name : $"{rule.Name} when {conditionName}";

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            // pos
            using (var pos = context.Emit.DeclareLocal<int>())
            {
                context.Emit.Duplicate(); // pos, pos
                context.Emit.StoreLocal(pos); // pos
                var localSuccess = context.Emit.DefineLabel();
                var fail = context.Emit.DefineLabel();
                bool canFail = rule.Emit(context, localSuccess);
                if (canFail)
                {
                    // Failed.
                    context.Emit.Branch(fail); // pos
                }

                context.Emit.MarkLabel(localSuccess); // v, pos
                // Success!  Need to verify condition.
                var condLocal = context.SaveInField(condition);
                using (var newPos = context.Emit.DeclareLocal<int>())
                using (var val = context.Emit.DeclareLocal<T>())
                {
                    // v, pos
                    context.Emit.StoreLocal(newPos); // v
                    context.Emit.StoreLocal(val); // ...
                    context.Emit.LoadField(condLocal); // cond
                    context.Emit.LoadLocal(val); // cond, v
                    context.Emit.CallVirtual(typeof(Func<T, bool>).GetMethod("Invoke", new[] { typeof(T) })); // cond_success

                    var condSuccess = context.Emit.DefineLabel();
                    context.Emit.BranchIfTrue(condSuccess); // ...
                    // Condition failed.  Load original pos and bail.
                    context.Emit.LoadLocal(pos); // pos
                    context.Emit.Branch(fail);

                    context.Emit.MarkLabel(condSuccess); // ...
                    // Success!  Reload v, pos and branch.
                    context.Emit.LoadLocal(val); // v
                    context.Emit.LoadLocal(newPos); // v, pos
                    context.Emit.Branch(success);
                }

                // Fail
                context.Emit.MarkLabel(fail); // pos
            }

            return true;
        }
    }
}
