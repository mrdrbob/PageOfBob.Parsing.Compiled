using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class StringRequiredRule : IRule<string>
    {
        private readonly IRule<string> rule;

        public StringRequiredRule(IRule<string> rule, string name)
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
            bool canFail = rule.Emit(context, localSuccess);
            if (canFail)
            {
                emit.Branch(end);
            }

            emit.MarkLabel(localSuccess); // v, pos
            using (var pos = emit.DeclareLocal<int>())
            {
                var requiredFail = emit.DefineLabel();

                emit.StoreLocal(pos); // v
                emit.Duplicate(); // v,  v
                emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // v, len
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
