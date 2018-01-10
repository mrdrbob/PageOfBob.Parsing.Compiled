using Sigil;
using System;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class TextFuncRule : AbstractRules.AbstractTextFuncRule<string>
    {
        public TextFuncRule(Func<char, bool> match, string name = null) : base(match, name) { }

        protected override void EmitSuccessLogic<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPosition)
        {
            context.EmitLoadString(pos, originalPosition);
        }
    }
}
