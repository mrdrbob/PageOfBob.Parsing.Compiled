using System;
using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class AlwaysRule<T> : IRule<T>
    {
        private readonly Func<T> createMethod;

        public AlwaysRule(Func<T> createMethod, string name = null)
        {
            this.createMethod = createMethod;
            Name = name ?? "Always";
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            using (var pos = emit.DeclareLocal<int>()) // pos
            {
                emit.StoreLocal(pos); // ...
                var field = context.SaveInField(createMethod);
                emit.LoadField(field); // meth
                emit.CallVirtual(typeof(Func<T>).GetMethod("Invoke", new Type[0]));  // v
                emit.LoadLocal(pos); // v, pos
            }

            emit.Branch(success);

            return false;
        }
    }
}
