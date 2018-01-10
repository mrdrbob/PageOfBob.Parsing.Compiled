using Sigil;

namespace PageOfBob.Parsing.Compiled.StringRules
{
    public class TextMatchRule : AbstractRules.AbstractTextMatchRule<string>
    {
        private readonly bool keepTextToMatch;

        public TextMatchRule(string textToMatch, bool caseSensitive, bool keepTextToMatch = true, string name = null) : base(textToMatch, caseSensitive, name)
        {
            this.keepTextToMatch = keepTextToMatch;
        }

        protected override void EmitLoadSuccess<TDelegate>(CompilerContext<TDelegate> context, Local pos, Local originalPos, string textToMatch)
        {
            if (keepTextToMatch)
            {
                context.Emit.LoadConstant(textToMatch);
            }
            else
            {
                context.EmitLoadString(pos, originalPos);
            }
        }
    }
}
