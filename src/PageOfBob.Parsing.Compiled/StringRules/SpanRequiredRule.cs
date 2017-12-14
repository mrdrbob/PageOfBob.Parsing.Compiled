using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class SpanRequiredRule : IRule<StringSpan>
    {
        private readonly IRule<StringSpan> rule;

        public SpanRequiredRule(IRule<StringSpan> rule, string name = null)
        {
            this.rule = rule;
            Name = name ?? $"(req:{rule.Name})";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;

            var fail = emit.DefineLabel();
            var localSuccess = emit.DefineLabel();
            // pos
            bool canFail = rule.Emit(context, localSuccess);
            if (canFail)
            {
                // Failed.
                emit.Branch(fail);
            }

            emit.MarkLabel(localSuccess);
            // v, pos
            using (var pos = emit.DeclareLocal<int>())
            {
                var requiredFail = emit.DefineLabel();

                emit.StoreLocal(pos); // v
                emit.Duplicate(); // v,  v
                using (var span = emit.DeclareLocal<StringSpan>())
                {
                    // TODO: Is there a better way to do this?
                    emit.StoreLocal(span); // v
                    emit.LoadLocalAddress(span); // v, v*
                    emit.Call(typeof(StringSpan).GetProperty("Length").GetMethod); // v, len
                }
                emit.BranchIfFalse(requiredFail); // v


                // Was success, put pos back on stack and exit.
                emit.LoadLocal(pos); // v, pos
                emit.Branch(success);

                // Fail!
                emit.MarkLabel(requiredFail); // v
                emit.Pop(); // ...
                emit.LoadLocal(pos); // pos
            }

            emit.MarkLabel(fail); // pos
            return true;
        }
    }
}
