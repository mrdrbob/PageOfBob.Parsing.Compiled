using Sigil;
using System;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class MatchFunctionRule : AbstractRules.AbstractMatchFunctionRule<char>
    {
        public MatchFunctionRule(Func<char, bool> match, string name = null) : base(match, name) { }

        protected override void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadChar(pos);
        }
    }
}
