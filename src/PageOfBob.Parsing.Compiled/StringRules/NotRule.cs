﻿using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class NotRule<K> : AbstractRules.AbstractNotRule<char, K>
    {
        public NotRule(IRule<K> rule, string name = null) : base(rule, name) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadChar(pos);
        }
    }
}
