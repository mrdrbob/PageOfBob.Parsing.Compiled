using Sigil;
using System;

namespace PageOfBob.Parsing.Compiled
{
    static class InternalExtensions
    {
        public static void MatchCharacterClass<TDelegate>(this CompilerContext<TDelegate> context, string charClass, Label success)
        {
            // c
            context.Emit.Call(typeof(char).GetMethod(charClass, new[] { typeof(char) })); // bool
            context.Emit.BranchIfTrue(success); // ...
        }

        public static void MatchCharacters<TDelegate>(this CompilerContext<TDelegate> context, bool caseSensistive, char[] charsToMatch, Label success)
        {
            var emit = context.Emit;
            var localSuccess = emit.DefineLabel();

            if (!caseSensistive)
            {
                emit.Call(typeof(char).GetMethod("ToUpperInvariant", new[] { typeof(char) })); // C
            }

            // c
            foreach (var c in charsToMatch)
            {
                emit.Duplicate(); // c, c
                var toLoad = caseSensistive ? c : char.ToUpperInvariant(c);
                emit.LoadConstant(toLoad); // c, c, c2
                emit.BranchIfEqual(localSuccess); // c
            }

            // Got here, fail.
            var end = emit.DefineLabel();
            emit.Pop(); // ...
            emit.Branch(end);

            emit.MarkLabel(localSuccess); // c
            emit.Pop(); // ...
            emit.Branch(success);

            emit.MarkLabel(end); // ...
        }

        public static void MatchFunction<TDelegate>(this CompilerContext<TDelegate> context, Func<char, bool> match, Label success)
        {
            var field = context.SaveInField(match);

            var emit = context.Emit;
            using (var c = emit.DeclareLocal<char>())
            {
                emit.StoreLocal(c); // ...
                emit.LoadField(field); // m
                emit.LoadLocal(c); // m, c
                emit.CallVirtual(typeof(Func<char, bool>).GetMethod("Invoke", new[] { typeof(char) })); // success
                emit.BranchIfTrue(success); // ...
            }
        }
    }
}
