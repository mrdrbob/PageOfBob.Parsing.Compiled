using System;
using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class MatchFunctionRule : AbstractRules.AbstractMatchFunctionRule<StringSpan>
    {
        public MatchFunctionRule(Func<char, bool> match, string name = null) : base(match, name) { }

        protected override void EmitSuccessObjectLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos)
        {
            context.EmitLoadStringSpan(pos);
        }
    }
}
