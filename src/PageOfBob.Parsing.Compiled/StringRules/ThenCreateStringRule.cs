using Sigil;
using System;
using System.Collections.Generic;
using System.Text;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class ThenCreateStringRule : IRule<string>
    {
        private readonly IRule<int> rule;

        public ThenCreateStringRule(IRule<int> rule, string name = null)
        {
            this.rule = rule;
            Name = name;
        }

        public string Name { get; }

        public bool Emit<TDelegate>(CompilerContext<TDelegate> context, Label success)
        {
            var emit = context.Emit;
            var localSuccess = emit.DefineLabel();
            var end = emit.DefineLabel();

            bool canFail = rule.Emit(context, localSuccess); // pos
            if (canFail)
            {
                emit.Branch(end); // pos
            }

            emit.MarkLabel(localSuccess);
            // start, pos
            using (var pos = emit.DeclareLocal<int>())
            using (var start = emit.DeclareLocal<int>())
            {
                emit.StoreLocal(pos); // start
                emit.StoreLocal(start); // ...
                context.EmitLoadString(pos, start); // v
                emit.LoadLocal(pos); // v, pos
                emit.Branch(success);
            }

            emit.MarkLabel(end);
            return canFail;
        }
    }
}
