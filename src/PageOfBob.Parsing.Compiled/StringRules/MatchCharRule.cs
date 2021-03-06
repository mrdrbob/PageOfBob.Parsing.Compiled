﻿using PageOfBob.Parsing.Compiled.AbstractRules;
using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class MatchCharRule : AbstractMatchCharRule<char>
    {
        public MatchCharRule(bool caseSensitive, params char[] charsToMatch) : base(caseSensitive, charsToMatch) { }

        protected override void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadChar(pos);
        }
    }
}
