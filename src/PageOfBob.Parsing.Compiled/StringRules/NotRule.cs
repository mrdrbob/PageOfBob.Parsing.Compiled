using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class NotRule<T> : AbstractRuleWithEndCheck<char>
    {
        private readonly IRule<T> rule;

        public NotRule(IRule<T> rule, string name)
        {
            this.rule = rule;
            Name = name ?? $"!{rule.Name}";
        }

        public override string Name { get; }

        protected override void EmitLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var localSuccess = emit.DefineLabel();
            using (var oPos = emit.DeclareLocal<int>())
            {
                // pos
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(oPos); // pos


                // Rule assumes CheckEofRule wraps it
                var canFail = rule.Emit(context, localSuccess);
                if (canFail) // pos
                {
                    // Fail is actually succeeding.
                    using (var pos = emit.DeclareLocal<int>())
                    {
                        emit.StoreLocal(pos); // ...
                        emit.LoadArgument(1); //  str
                        emit.LoadLocal(pos); // str, pos
                        emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // c
                        emit.LoadLocal(pos); // c, pos
                        emit.LoadConstant(1); // c, pos, 1
                        emit.Add(); // c, pos
                        emit.Branch(success);
                    }
                }

                // Rule succeeded, so it actually failed.
                emit.MarkLabel(localSuccess); //v2, pos2
                emit.Pop(); // v2
                emit.Pop(); // ...
                emit.LoadLocal(oPos); // pos
            }
        }
    }
}
