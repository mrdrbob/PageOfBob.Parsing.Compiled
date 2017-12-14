using Sigil;
using System.Text;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class ManyAsTextRule : IRule<string>
    {
        private readonly IRule<char> rule;

        public ManyAsTextRule(IRule<char> rule, string name)
        {
            this.rule = rule;
            Name = name ?? $"{rule.Name}.ToString()";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;

            using (Local sb = emit.DeclareLocal<StringBuilder>())
            using (var pos = emit.DeclareLocal<int>())
            {
                emit.NewObject<StringBuilder>();
                emit.StoreLocal(sb);

                // pos
                var start = emit.DefineLabel();
                var end = emit.DefineLabel();
                var localSuccess = emit.DefineLabel();

                // Start of the loop
                emit.MarkLabel(start);

                // Try the rule
                bool canFail = rule.Emit(context, localSuccess);
                if (canFail)
                {
                    // This rule failed.
                    emit.Branch(end); // pos
                }

                emit.MarkLabel(localSuccess);
                // This rule succeeded.
                // c, pos
                emit.StoreLocal(pos); // c
                using (var c = emit.DeclareLocal<char>())
                {
                    emit.StoreLocal(c); // ...
                    emit.LoadLocal(sb); // sb
                    emit.LoadLocal(c); // sb, c
                    emit.CallVirtual(typeof(StringBuilder).GetMethod("Append", new[] { typeof(char) })); // sb
                    emit.Pop(); // ...
                    emit.LoadLocal(pos); // pos
                }


                // Back to the top of the loop
                emit.Branch(start);


                // End of the loop
                emit.MarkLabel(end); // pos
                emit.StoreLocal(pos); // ...
                emit.LoadLocal(sb); // sb
                emit.CallVirtual(typeof(StringBuilder).GetMethod("ToString", new System.Type[0])); // v2
                emit.LoadLocal(pos); // v, pos
            }

            emit.Branch(success);
            return false;
        }
    }
}
