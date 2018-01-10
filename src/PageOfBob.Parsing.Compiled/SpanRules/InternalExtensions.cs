using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    static class InternalExtensions
    {
        public static void EmitLoadStringSpan<TDelegate>(this CompilerContext<TDelegate> context, Local pos)
        {
            context.Emit.LoadArgument(1); // str
            context.Emit.LoadLocal(pos); // str, pos
            context.Emit.Duplicate(); // str, pos, pos
            context.Emit.LoadConstant(1); // str, pos, pos, 1
            context.Emit.Add(); // str, pos, pos+1
            context.Emit.NewObject(typeof(StringSpan).GetConstructor(new[] { typeof(string), typeof(int), typeof(int) })); // v
        }

        public static void EmitLoadStringSpan<TDelegate>(this CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            // ...
            context.Emit.LoadArgument(1); // str
            context.Emit.LoadLocal(originalPosition); // str, opos
            context.Emit.LoadLocal(pos); // str, opos, pos

            context.Emit.NewObject(typeof(StringSpan).GetConstructor(new[] { typeof(string), typeof(int), typeof(int) })); // v
        }

    }
}
