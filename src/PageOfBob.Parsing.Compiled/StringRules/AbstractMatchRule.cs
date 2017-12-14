using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public abstract class AbstractMatchRule : AbstractRuleWithEndCheck<char>
    {
        protected abstract void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label match);

        protected override void EmitLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var fail = emit.DefineLabel();

            emit.Duplicate(); // pos, pos
            using (var pos = emit.DeclareLocal<int>())
            {
                emit.StoreLocal(pos); // pos
                emit.LoadArgument(1); // pos, str
                emit.LoadLocal(pos); // pos, str, pos
                emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // pos, c

                var localSuccess = emit.DefineLabel();

                EmitMatchLogic(context, localSuccess);

                // Did not match.
                // Expected: pos
                emit.Branch(fail);

                // Did match
                emit.MarkLabel(localSuccess);
                // pos, v2
                using (var val = emit.DeclareLocal<char>())
                {
                    emit.StoreLocal(val); //pos
                    emit.Pop(); // ...
                    emit.LoadLocal(val); // v2
                    emit.LoadLocal(pos); // v2, pos
                    emit.LoadConstant(1); // v2, pos, 1
                    emit.Add(); // v2, newpos
                }

                emit.Branch(success);
            }

            emit.MarkLabel(fail); // pos
        }
    }
}
