using Sigil;

namespace PageOfBob.Parsing.Compiled.AbstractRules
{
    public abstract class AbstractTextMatchRule<T> : IRule<T>
    {
        private readonly string textToMatch;
        private readonly bool caseSensitive;

        public AbstractTextMatchRule(string textToMatch, bool caseSensitive, string name = null)
        {
            this.textToMatch = textToMatch;
            this.caseSensitive = caseSensitive;
            Name = name ?? $"\"{textToMatch}\"";
        }

        public string Name { get; }

        protected abstract void EmitLoadSuccess<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPos, string textToMatch);

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var fail = emit.DefineLabel();

            // Check is there's enough string left for this
            emit.Duplicate(); // pos, pos
            emit.LoadConstant(textToMatch.Length); // pos, pos, expLen
            emit.Add(); // pos, minLen
            emit.LoadLocal(context.LengthLocal); // pos, minLen, len
            emit.BranchIfGreater(fail); // minPos > len -- too short.

            using (var oPos = emit.DeclareLocal<int>())
            using (var pos = emit.DeclareLocal<int>())
            {
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(oPos); // pos
                emit.StoreLocal(pos); // ...

                foreach(var c in textToMatch)
                {
                    var localSuccess = emit.DefineLabel();
                    emit.LoadLocal(context.StringLocal); // str
                    emit.LoadLocal(pos); // str, pos
                    emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // c
                    if (!caseSensitive)
                        emit.Call(typeof(char).GetMethod("ToUpperInvariant", new[] { typeof(char) })); // C
                    var toLoad = caseSensitive ? c : char.ToUpperInvariant(c);
                    emit.LoadConstant(toLoad); // c, c2
                    emit.BranchIfEqual(localSuccess); // ...

                    // Didn't match, put original pos back on stack and bail out.
                    emit.LoadLocal(oPos);
                    emit.Branch(fail);

                    // Matched
                    emit.MarkLabel(localSuccess);
                    emit.LoadLocal(pos); // pos
                    emit.LoadConstant(1); // pos, 1
                    emit.Add(); // pos2
                    emit.StoreLocal(pos); // ...
                }

                // Got here, must have been successful
                EmitLoadSuccess(context, pos, oPos, textToMatch); // v
                emit.LoadLocal(pos); // v, pos
                emit.Branch(success);
            }

            emit.MarkLabel(fail);
            return true;
        }
    }
}
