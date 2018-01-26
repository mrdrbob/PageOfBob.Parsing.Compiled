using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractTextRule<T> : IRule<T>
    {
        public abstract string Name { get; }

        protected abstract void EmitMatchLogic<TDelegate>(CompilerContext<TDelegate> context, Label matchSuccess);

        protected abstract void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition);

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var end = emit.DefineLabel();

            using (var oPos = emit.DeclareLocal<int>())
            {
                // pos
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(oPos); // pos

                // START LOOP
                var loopStart = emit.DefineLabel(); 
                emit.MarkLabel(loopStart); // pos

                // Check len
                emit.Duplicate(); //pos, pos
                emit.LoadLocal(context.LengthLocal); // pos, pos, len
                emit.BranchIfGreaterOrEqual(end); // pos

                using (var pos = emit.DeclareLocal<int>())
                {
                    emit.StoreLocal(pos); // ...
                    emit.LoadLocal(context.StringLocal); // str
                    emit.LoadLocal(pos); // str, pos
                    emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // c

                    // Do check
                    var matchSuccess = emit.DefineLabel();
                    EmitMatchLogic(context, matchSuccess);
                    
                    // Failed.
                    emit.LoadLocal(pos); // pos
                    emit.Branch(end);

                    // Match success
                    emit.MarkLabel(matchSuccess);
                    emit.LoadLocal(pos); // pos
                    emit.LoadConstant(1); // pos, 1
                    emit.Add(); // newPos
                }

                // Loop
                emit.Branch(loopStart); // newPos

                // End of loop
                emit.MarkLabel(end); // newPos

                // Put together final product
                using (var pos = emit.DeclareLocal<int>())
                {
                    emit.StoreLocal(pos); // ...
                    EmitSuccessLogic(context, pos, oPos); // v
                    emit.LoadLocal(pos); // pos
                }
                emit.Branch(success);
            }

            return false;
        }
    }
}
