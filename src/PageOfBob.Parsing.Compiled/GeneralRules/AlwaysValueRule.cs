using Sigil;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class AlwaysValueRule<T> : IRule<T>
    {
        private readonly T value;

        public AlwaysValueRule(T value, string name = null)
        {
            this.value = value;
            Name = name ?? value.ToString();
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            using (var pos = emit.DeclareLocal<int>()) // pos
            {
                emit.StoreLocal(pos); // ...
                var field = context.SaveInField(value);
                emit.LoadField(field); // v
                emit.LoadLocal(pos); // v, pos
            }

            emit.Branch(success);

            return false;
        }
    }
}
