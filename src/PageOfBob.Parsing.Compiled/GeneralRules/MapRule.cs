using Sigil;
using System;

namespace PageOfBob.Parsing.Compiled.GeneralRules
{
    public class MapRule<T, O> : IRule<O>
    {
        private readonly IRule<T> rule;
        private readonly Func<T, O> map;

        public MapRule(IRule<T> rule, Func<T, O> map, string name)
        {
            this.rule = rule;
            this.map = map;
            Name = name;
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var end = emit.DefineLabel();
            var localSuccess = emit.DefineLabel();

            bool canFail = rule.Emit(context, localSuccess);
            if (canFail)
            {
                // failed, end.
                emit.Branch(end);
            }

            // Success!
            emit.MarkLabel(localSuccess); // v, pos
            using (var pos = emit.DeclareLocal<int>())
            using (var val = emit.DeclareLocal<T>())
            {
                emit.StoreLocal(pos); // v
                emit.StoreLocal(val); // ...
                var mapField = context.SaveInField(map);
                emit.LoadField(mapField); // map
                emit.LoadLocal(val); // map, v
                emit.CallVirtual(typeof(Func<T, O>).GetMethod("Invoke", new[] { typeof(T) })); // v2
                emit.LoadLocal(pos); // v2, pos
                emit.Branch(success);
            }

            emit.MarkLabel(end);
            return canFail;
        }
    }
}
