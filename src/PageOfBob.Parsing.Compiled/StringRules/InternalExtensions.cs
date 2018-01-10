using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    static class InternalExtensions
    {
        public static void EmitLoadChar<TDelegate>(this CompilerContext<TDelegate> context, Local pos)
        {
            context.Emit.LoadArgument(1); // str
            context.Emit.LoadLocal(pos); // str, pos
            context.Emit.CallVirtual(typeof(string).GetMethod("get_Chars", new[] { typeof(int) })); // c
        }

        public static void EmitLoadString<TDelegate>(this CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.Emit.LoadArgument(1); // str
            context.Emit.LoadLocal(originalPosition); // str, opos
            context.Emit.LoadLocal(pos); // str, opos, pos
            context.Emit.LoadLocal(originalPosition); // str, opos, pos, opos
            context.Emit.Subtract(); // str, opos, len
            context.Emit.CallVirtual(typeof(string).GetMethod("Substring", new[] { typeof(int), typeof(int) }));
        }
    }
}
