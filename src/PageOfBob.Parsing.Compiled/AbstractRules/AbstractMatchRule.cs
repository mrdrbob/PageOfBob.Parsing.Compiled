using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    /// <summary>
    /// Abstract rule for matching single item
    /// </summary>
    public abstract class AbstractMatchRule<T> : AbstractRuleWithEndCheck<T>
    {
        protected abstract void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label success);

        protected abstract void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos);

        protected override void EmitLogic<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var fail = emit.DefineLabel();

            using (var pos = emit.DeclareLocal<int>())
            {
                emit.StoreLocal(pos); // ...
                emit.LoadLocal(context.StringLocal); // str
                emit.LoadLocal(pos); // str, pos
                emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // c

                var localSuccess = emit.DefineLabel();

                EmitMatchLogic(context, localSuccess); // c

                // Did not match.
                // ...
                emit.LoadLocal(pos); // pos
                emit.Branch(fail);

                // Did match
                emit.MarkLabel(localSuccess); // ...

                EmitSuccessObjectLogic(context, pos); // v

                emit.LoadLocal(pos); // v, pos
                emit.LoadConstant(1); // v, pos, 1
                emit.Add(); // v, pos2

                emit.Branch(success);
            }

            emit.MarkLabel(fail); // pos
        }
    }
}
