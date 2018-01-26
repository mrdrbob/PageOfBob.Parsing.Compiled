using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class ThenCreateSpanRule : IRule<StringSpan>
    {
        private readonly IRule<int> rule;

        public ThenCreateSpanRule(IRule<int> rule, string name = null)
        {
            this.rule = rule;
            Name = name;
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var localSuccess = emit.DefineLabel();
            var end = emit.DefineLabel();

            bool canFail = rule.Emit(context, localSuccess); // pos
            if (canFail)
            {
                emit.Branch(end); // pos
            }

            emit.MarkLabel(localSuccess);
            // start, pos
            using (var pos = emit.DeclareLocal<int>())
            using (var start = emit.DeclareLocal<int>())
            {
                emit.StoreLocal(pos); // start
                emit.StoreLocal(start); // ...
                context.EmitLoadStringSpan(pos, start); // v
                emit.LoadLocal(pos); // v, pos
                emit.Branch(success);
            }

            emit.MarkLabel(end);
            return canFail;
        }
    }
}
