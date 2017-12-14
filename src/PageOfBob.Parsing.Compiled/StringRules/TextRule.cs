using System;
using System.Collections.Generic;
using System.Text;
using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class TextRule : IRule<string>
    {
        private readonly string expectedValue;
        private readonly bool caseInsensitive;

        public TextRule(string expectedValue, bool caseInsensitive, string name)
        {
            this.expectedValue = expectedValue;
            this.caseInsensitive = caseInsensitive;
            Name = name ?? $"\"{expectedValue}\"";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            // pos
            var emit = context.Emit;
            var end = emit.DefineLabel();

            // Cheating here, since we're working with a string input.
            emit.Duplicate(); // pos, pos
            emit.LoadConstant(expectedValue.Length); // pos, pos, expLen
            emit.Add(); // pos, minLen
            emit.LoadArgument(1); // pos, minLen, string
            emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // pos, minLen, len
            emit.BranchIfGreater(end); // minPos > len -- too short.

            // Not too short
            using (var opos = emit.DeclareLocal<int>())
            {
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(opos); // pos

                foreach (var c in expectedValue)
                {
                    using (var pos = emit.DeclareLocal<int>())
                    {
                        var localSuccess = emit.DefineLabel(); // pos
                        emit.StoreLocal(pos); // ...
                        emit.LoadArgument(1); // str
                        emit.LoadLocal(pos); // str, pos
                        emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // c
                        if (caseInsensitive)
                            emit.Call(typeof(char).GetMethod("ToUpperInvariant", new[] { typeof(char) }));
                        emit.LoadConstant(caseInsensitive ? char.ToUpperInvariant(c) : c); // c, exc
                        emit.BranchIfEqual(localSuccess); // ...

                        // Didn't match, put original pos back on stack and bail out.
                        emit.LoadLocal(opos);
                        emit.Branch(end);

                        // Succeeded, get the next char
                        emit.MarkLabel(localSuccess);
                        emit.LoadLocal(pos); // pos
                        emit.LoadConstant(1); // pos, 1
                        emit.Add(); // pos2
                    }
                }

                // Got here, must have been successful.
                using (var finalPos = emit.DeclareLocal<int>()) // pos2
                {
                    emit.StoreLocal(finalPos); // ...
                    emit.LoadConstant(expectedValue); // str
                    emit.LoadLocal(finalPos); // str, pos
                    emit.Branch(success);
                }
            }

            emit.MarkLabel(end);
            return true;
        }
    }
}
