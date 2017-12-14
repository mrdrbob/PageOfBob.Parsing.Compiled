using Sigil;
using System.Collections.Generic;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class RequiredRule<T> : IRule<List<T>>
    {
        private readonly IRule<List<T>> rule;

        public RequiredRule(IRule<List<T>> rule, string name)
        {
            this.rule = rule;
            Name = name ?? $"(req:{rule.Name})";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var end = emit.DefineLabel();
            var localSuccess = emit.DefineLabel();

            // pos
            bool canFail = rule.Emit(context, localSuccess);
            if (canFail) // pos
            {
                // Failed!
                emit.Branch(end);
            }

            emit.MarkLabel(localSuccess);
            // Success!
            // v, pos
            using (var pos = emit.DeclareLocal<int>())
            {
                var requiredFail = emit.DefineLabel();

                emit.StoreLocal(pos); // v
                emit.Duplicate(); // v,  v
                emit.CallVirtual(typeof(List<T>).GetProperty("Count").GetMethod); // v, count
                emit.BranchIfFalse(requiredFail); // v

                // Was success, put pos back on stack and exit.
                emit.LoadLocal(pos); // v, pos
                emit.Branch(success);

                // Fail!
                emit.MarkLabel(requiredFail); // v
                emit.Pop(); // ...
                emit.LoadLocal(pos); // pos
            }

            emit.MarkLabel(end);

            return true;
        }
    }
}
