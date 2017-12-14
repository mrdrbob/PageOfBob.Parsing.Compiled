using System;
using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class MatchSpanRule : IRule<StringSpan>
    {
        private readonly Func<char, bool> match;

        public MatchSpanRule(Func<char, bool> match, string name = null)
        {
            this.match = match;
            Name = name ?? "Span";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;

            var fail = emit.DefineLabel();

            var matchField = context.SaveInField(match);

            using (var originalPos = emit.DeclareLocal<int>())
            using (var len = emit.DeclareLocal<int>())
            {
                // pos
                emit.Duplicate(); // pos, pos
                emit.StoreLocal(originalPos); // pos

                emit.LoadArgument(1); // pos, str
                emit.CallVirtual(typeof(string).GetProperty("Length").GetMethod); // pos, len
                emit.StoreLocal(len); // pos

                // START LOOP
                var loopStart = emit.DefineLabel();
                emit.MarkLabel(loopStart);

                emit.Duplicate(); //pos, pos
                emit.LoadLocal(len); // pos, pos, len
                emit.BranchIfGreaterOrEqual(fail); // pos

                using (var pos = emit.DeclareLocal<int>())
                {
                    emit.Duplicate(); // pos, pos
                    emit.StoreLocal(pos); // pos 
                    emit.LoadField(matchField); // pos, m
                    emit.LoadArgument(1); // pos, m, str
                    emit.LoadLocal(pos); // pos, m, str, pos
                    emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // pos, m, c
                    emit.CallVirtual(typeof(Func<char, bool>).GetMethod("Invoke", new[] { typeof(char) })); // pos, success
                    emit.BranchIfFalse(fail); // pos
                    // Success!
                    emit.LoadConstant(1); //pos, 1
                    emit.Add(); // newPos
                }

                emit.Branch(loopStart); // newPos

                // FAIL
                emit.MarkLabel(fail); // newPos

                // Put together object, jump to success
                using (var newPos = emit.DeclareLocal<int>())
                {
                    emit.StoreLocal(newPos); // ...
                    emit.LoadArgument(1); // str
                    emit.LoadLocal(originalPos); // str, opos
                    emit.LoadLocal(newPos); // str, opos, npos
                    emit.NewObject(typeof(StringSpan).GetConstructor(new[] { typeof(string), typeof(int), typeof(int) })); // v
                    emit.LoadLocal(newPos); // v, pos
                }

                emit.Branch(success);
            }

            return false;
        }
    }
}
