using System;
using Sigil;

namespace PageOfBob.Parsing.Compiled.SpanRules
{
    public class TextFuncRule : AbstractRules.AbstractTextFuncRule<StringSpan>
    {
        public TextFuncRule(Func<char, bool> match, string name = null) : base(match, name) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.EmitLoadStringSpan(pos, originalPosition);
        }
    }
}
