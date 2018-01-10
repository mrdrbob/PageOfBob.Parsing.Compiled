using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractNotTextRule<T, K> : IRule<T>
    {
        private readonly IRule<K> rule;

        protected AbstractNotTextRule(IRule<K> rule, string name)
        {
            this.rule = rule;
            Name = name ?? $"!{rule.Name}";
        }

        public string Name { get; }

        protected abstract void EmitLoadSuccess<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPos);

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var end = emit.DefineLabel();

            // pos
            using (var oPos = emit.DeclareLocal<int>())
            using (var pos = emit.DeclareLocal<int>())
            using (var len = emit.DeclareLocal<int>())
            {
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(pos); // pos
                emit.StoreLocal(oPos); // ...

                emit.LoadArgument(1); // str
                emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // len
                emit.StoreLocal(len); // ...

                // Start
                var start = emit.DefineLabel(); // ...
                var localSuccess = emit.DefineLabel();
                emit.MarkLabel(start);

                emit.LoadLocal(pos); // pos
                emit.LoadLocal(len); // pos, len
                emit.BranchIfGreaterOrEqual(end); // ...

                emit.LoadLocal(pos); // pos

                bool canFail = rule.Emit(context, localSuccess);
                if (canFail)
                {
                    // Fail, actually a success, pos should be original pos
                    // Add one and loop
                    emit.LoadConstant(1); // pos, 1
                    emit.Add(); // pos+1
                    emit.StoreLocal(pos); // ...
                    emit.Branch(start);
                }

                emit.MarkLabel(localSuccess); // npos, v
                // Success is actually a fail.
                emit.Pop(); // npos
                emit.Pop(); // ...

                emit.MarkLabel(end);

                EmitLoadSuccess(context, pos, oPos); // v
                emit.LoadLocal(pos); // v, pos
                emit.Branch(success);
            }

            return false;
        }
    }
}
