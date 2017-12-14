using System;
using System.Collections.Generic;
using System.Text;
using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class OptionalRule<T> : IRule<T>
    {
        private readonly IRule<T> rule;
        private readonly T defaultValue;

        public OptionalRule(IRule<T> rule, T defaultValue)
        {
            this.rule = rule;
            this.defaultValue = defaultValue;
        }

        public string Name => $"OPT:{rule.Name}";

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            // pos
            bool canFail = rule.Emit(context, success);
            if (canFail)
            {
                // If we got here, the rule can fail and did fail.
                using (var pos = context.Emit.DeclareLocal<int>())
                {
                    context.Emit.StoreLocal(pos); // ...
                    var defValLocal = context.SaveInField(defaultValue);
                    context.Emit.LoadField(defValLocal);  // val
                    context.Emit.LoadLocal(pos); // val, pos
                    context.Emit.Branch(success);
                }
            }

            return false;
        }
    }
}
